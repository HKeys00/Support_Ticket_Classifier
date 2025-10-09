#!/usr/bin/env python
# coding: utf-8

# In[1]:


# This Python 3 environment comes with many helpful analytics libraries installed
# It is defined by the kaggle/python Docker image: https://github.com/kaggle/docker-python
# For example, here's several helpful packages to load

import numpy as np # linear algebra
import pandas as pd # data processing, CSV file I/O (e.g. pd.read_csv)

# Input data files are available in the read-only "../input/" directory
# For example, running this (by clicking run or pressing Shift+Enter) will list all files under the input directory

import os
for dirname, _, filenames in os.walk('/kaggle/input'):
    for filename in filenames:
        print(os.path.join(dirname, filename))

# You can write up to 20GB to the current directory (/kaggle/working/) that gets preserved as output when you create a version using "Save & Run All" 
# You can also write temporary files to /kaggle/temp/, but they won't be saved outside of the current session


# **First we must frame the problem:**
# 
# How does the business/users expect to use and benefit from this model:
# Managers want to free up time spent organising and working out the priority of a ticket. Using a model to predict what priority a ticket would be frees up support worker time to focus on actually closing tickets.
# 
# What kind of training supervision will the model need:
# This model will need supervised training as the data fed to the algorithm will include the desired solution
# 
# Is this a regression or classification task:
# This will be a classification task, based on input features we will need to classify the priority of a ticket.
# 
# Batch learning or Online learning:
# Batch learning because the data is stable with infrequent model updates.
# 

# **Select a performance measure:**
# 
# Because we are predicting a classification outcome we should start with F1 Score, the F1 score is a number that tells how good the classifier is at finding the positive in each class while balancing precision (How many tickets did the model predict the priority of correctly) and recall (How many of each priority was caught)

# # GET THE DATA

# In[ ]:

ticket = pd.read_csv("customer_support_tickets.csv")
ticket.drop(columns=['Customer Name', 'Customer Email', 'Customer Age', 'Customer Gender', 'Product Purchased', 'Resolution',
                    'Ticket Status', 'First Response Time', 'Time to Resolution', 'Customer Satisfaction Rating'], inplace=True,  errors='ignore')

# # CREATE A TEST SET
# 
# Set apart a random 20% of the dataset for later use.

# In[ ]:

from zlib import crc32

def is_id_in_test_set(identifier, test_ratio):
    return crc32(np.int64(identifier)) < test_ratio * 2**32 #crc32 computes the crc32 hash of the input number
                                                            #produces a 32-bit unsinged integer hash value - basically a pseudo-random number
def split_data_with_id_hash(data, test_ratio, id_column):
    ids = data[id_column]
    in_test_set = ids.apply(lambda id_: is_id_in_test_set(id_, test_ratio))
    return data.loc[~in_test_set], data.loc[in_test_set]
    # df.loc Selects rows meeting logical condition, ~ means select all that aren't in the test_set 

train_set, test_set = split_data_with_id_hash(ticket, 0.2, "Ticket Id") #Ticket has a unique identifier so use that

# This has used random sampling, I think a samf assumption can be made that the Ticket Type heavily impacts the ticket priority, so it might be worth doing statified sampling where we will seperate the dataset into srata (ticket types) to make sure that the right number of instances are sampled from each stratum
# 
# First we have to convert the ticket type column into discrete numbers


from sklearn.preprocessing import LabelEncoder

le = LabelEncoder()
ticket_type_col = le.fit_transform(ticket["Ticket Type"])

list(zip(le.classes_, range(len(le.classes_))))



# The following code generates 10 different stratified splits of the same dataset.
# 
# - Multiple splits let you evaluate your model more robustly through cross-validation. Each split serves as a different train/test combination.
# 
# So what is occuring here?
# The dataset is being divided into two subsets, Training and Test but the StratifiedShuffleSplit ensures that the split is randomized, but reproducible with random_state, and each subset retains the same class distribution.

from sklearn.model_selection import StratifiedShuffleSplit

def split_data_with_strat():
    splitter = StratifiedShuffleSplit(n_splits=10, test_size=0.2, random_state=42)
    strat_splits=[]
    for train_index, test_index in splitter.split(ticket, ticket["Ticket Type"]):
        strat_train_set_n = ticket.iloc[train_index]
        strat_test_set_n = ticket.iloc[test_index]
        strat_splits.append([strat_train_set_n, strat_test_set_n])
    strat_train_set, strat_test_set = strat_splits[0]
    return strat_train_set, strat_test_set # Just use the first split for now


# # CLEAN THE DATA
# This data has plenty of missing features these should be set to the median value.
# 
# All the columns that are being dropped do not serve the purpose of classifying the priority of support tickets, they may be added back if something else needs to be done with the data. Notice how the data is split after dropping the columns but before any data transforming takes place.


strat_train_set, strat_test_set = split_data_with_strat()
for set_ in (strat_train_set, strat_test_set):
    set_.drop("Ticket Id", axis=1, inplace=True)
strat_train_set.info()


# Categorical attributes, such as priority, type and channel can be encoded, but we must be careful how we encode each one.
# For ordered categories such as the ticket priority, these values can simply be encoded, but for none ordered like the ticket type and channel these should be OneHotEncoded. 
# 
# The following code demonstrates how this may be done, it doesn't actually transform the data.


from sklearn.preprocessing import OrdinalEncoder

priority_order = [["Low", "Medium", "High", "Critical"]]

ticket_priority_cat = ticket[["Ticket Priority"]]
ordinal_encoder = OrdinalEncoder(categories=priority_order)
ticket_priority_cat_encoded = ordinal_encoder.fit_transform(ticket_priority_cat)

# # MAKE A PIPELINE
# 
# Many data transformation steps need to be executed in a specific order, so a pipeline needs to be created

# In[ ]:


from sklearn.compose import ColumnTransformer
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import OneHotEncoder
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.pipeline import make_pipeline
from sklearn.impute import SimpleImputer

one_hot_cat_attribs = ["Ticket Channel", "Ticket Type" ]

one_hot_pipeline = make_pipeline(
    SimpleImputer(strategy="most_frequent"),
    OneHotEncoder(handle_unknown="ignore")
)

preprocesser = ColumnTransformer([
    ("text_desc", TfidfVectorizer(max_features=1000, stop_words="english"), "Ticket Description"),
    ("text_sub", TfidfVectorizer(max_features=1000, stop_words="english"), "Ticket Subject"),
    ("hot", one_hot_pipeline, one_hot_cat_attribs)
])


# Now our pipeline has been built we *simply* train the model
# 
# - x_train is the training feature set, includes all columns except the target column
# - y_train is the training target set, includes only the target column
# - x_test is the test feature set, is the inputs the model will use to make predictions
# - y_test is the test target set, is the labels for those inputs

from sklearn import svm
from sklearn.preprocessing import StandardScaler
from sklearn.preprocessing import FunctionTransformer

def to_dense_transform(x):
    return x.toarray() if hasattr(x, "toarray") else x

pipeline = Pipeline([
    ("preprocessing", preprocesser),
    ("to_dense", FunctionTransformer(to_dense_transform)),
    ("scaler", StandardScaler(with_mean=False)),
    ("svm", svm.SVC(probability=True))
])

priority_order = [["Low", "Medium", "High", "Critical"]]
label_encoder = OrdinalEncoder(categories=priority_order)

x_train = strat_train_set.drop("Ticket Priority", axis=1)
y_train = strat_train_set["Ticket Priority"]
x_test = strat_test_set.drop("Ticket Priority", axis=1)
y_test = strat_test_set["Ticket Priority"]

y_train_encoded = label_encoder.fit_transform(y_train.values.reshape(-1, 1))
y_test_encoded = label_encoder.transform(y_test.values.reshape(-1, 1))

pipeline.fit(x_train, y_train_encoded.ravel())

import joblib
joblib.dump(pipeline, "ticket_classifier_model.pkl")
joblib.dump(label_encoder, "label_encoder.pkl")