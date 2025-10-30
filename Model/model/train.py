import os
import pandas as pd
from pathlib import Path
from sklearn.preprocessing import OneHotEncoder, LabelEncoder, FunctionTransformer
from sklearn.model_selection import StratifiedShuffleSplit
from sklearn.compose import ColumnTransformer
from sklearn.pipeline import Pipeline
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.pipeline import make_pipeline
from sklearn.linear_model import SGDClassifier
import joblib

# ------------------------------------------------------
# CONFIG
DATASET_PATH = "./customer_support_tickets.csv"
MODEL_VERSION = "00001"
MODEL_NAME = "./ticket_classifier_model"
MODEL_OUT = MODEL_VERSION + ".pkl"
MODEL_PATH = Path(MODEL_NAME) / MODEL_OUT
# ------------------------------------------------------

os.makedirs(os.path.dirname(MODEL_PATH), exist_ok=True)

def squeeze_column(x):
    return x.squeeze()

ticket = pd.read_csv(DATASET_PATH)
ticket.drop(columns=['Customer Name', 'Customer Email', 'Customer Age', 'Customer Gender', 'Product Purchased', 'Resolution',
                    'Ticket Status', 'First Response Time', 'Time to Resolution', 'Customer Satisfaction Rating'], inplace=True,  errors='ignore')
ticket.dropna(inplace=True)
X = ticket.drop(columns=["Ticket Priority"])
Y = ticket["Ticket Priority"]

splitter = StratifiedShuffleSplit(n_splits=1, test_size=0.2, random_state=42)
for train_index, test_index in splitter.split(X, Y):
    x_train, x_test = X.iloc[train_index], X.iloc[test_index]
    y_train, y_test = Y.iloc[train_index], Y.iloc[test_index]

le = LabelEncoder()
y_train_encoded = le.fit_transform(y_train)
y_test_encoded = le.transform(y_test)

cat_cols = ["Ticket Channel", "Ticket Type"]

preprocessor = ColumnTransformer([
    ("cat", OneHotEncoder(handle_unknown="ignore"), cat_cols),
    ("desc_tfidf", Pipeline([
        ("extract_column", FunctionTransformer(squeeze_column, validate=False)),
        ("tfidf", TfidfVectorizer(max_features=1000, stop_words="english"))
    ]), ["Ticket Description"]),
    ("subject_tfidf", Pipeline([
        ("extract_column", FunctionTransformer(squeeze_column, validate=False)),
        ("tfidf", TfidfVectorizer(max_features=1000, stop_words="english"))
    ]), ["Ticket Subject"])
])

model = Pipeline([
    ("preprocessing", preprocessor),
    ("clf", SGDClassifier(loss="log_loss", max_iter=1000, tol=1e-3, random_state=42))
])

model.fit(x_train, y_train_encoded)
predictions = model.predict(x_test)

print (x_test.head())
print(predictions)
#joblib.dump(model, MODEL_PATH)
#joblib.dump(label_encoder, "label_encoder.pkl")