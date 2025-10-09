import pandas as pd
import numpy as np
from transformers import AutoTokenizer, AutoModelForSequenceClassification, Trainer, TrainingArguments
from sklearn.preprocessing import OrdinalEncoder
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, f1_score

# ------------------------------------------------------
# CONFIG
DATASET_PATH = "./customer_support_tickets.csv"
MODEL_OUT = "./hugging_face_ticket_priority_model"
MAX_LEN = 256
NUM_LABELS = 4
# ------------------------------------------------------

def load_data():
    df = pd.read_csv(DATASET_PATH)
    # drop columns you don’t need
    df.drop(columns=[
        'Customer Name', 'Customer Email', 'Customer Age', 'Customer Gender',
        'Product Purchased', 'Resolution', 'Ticket Status', 'First Response Time',
        'Time to Resolution', 'Customer Satisfaction Rating'
    ], inplace=True, errors="ignore")
    return df

def preprocess_datasets(df):
    # Encode labels in priority order
    priority_order = [["Low", "Medium", "High", "Critical"]]
    label_encoder = OrdinalEncoder(categories=priority_order)
    y = label_encoder.fit_transform(df[["Ticket Priority"]]).ravel()

    X_train, X_test, y_train, y_test = train_test_split(
        df, y, test_size=0.2, stratify=df["Ticket Priority"], random_state=42
    )

    train_dataset = Dataset.from_dict({
        "text": X_train["Ticket Description"].tolist(),
        "label": y_train.tolist()
    })

    val_dataset = Dataset.from_dict({
        "text": X_test["Ticket Description"].tolist(),
        "label": y_test.tolist()
    })

    return train_dataset, val_dataset, label_encoder

def compute_metrics(pred):
    labels = pred.label_ids
    preds = np.argmax(pred.predictions, axis=1)
    return {
        "accuracy": accuracy_score(labels, preds),
        "f1": f1_score(labels, preds, average="weighted")
    }

def train_model():
    df = load_data()
    train_dataset, val_dataset, label_encoder = preprocess_datasets(df)

    tokenizer = AutoTokenizer.from_pretrained("distilbert-base-uncased")

    def tokenize_fn(examples):
        return tokenizer(examples["text"], truncation=True, padding="max_length", max_length=MAX_LEN)

    train_dataset = train_dataset.map(tokenize_fn, batched=True)
    val_dataset = val_dataset.map(tokenize_fn, batched=True)

    model = AutoModelForSequenceClassification.from_pretrained(
        "distilbert-base-uncased",
        num_labels=NUM_LABELS
    )

    training_args = TrainingArguments(
        output_dir="./results",
        num_train_epochs=3,
        per_device_train_batch_size=16,
        per_device_eval_batch_size=32,
        learning_rate=2e-5,
        evaluation_strategy="epoch",   # NOTE: use 'evaluation_strategy' not 'eval_strategy'
        save_strategy="epoch",
        logging_dir="./logs",
        logging_steps=50,
    )

    trainer = Trainer(
        model=model,
        args=training_args,
        train_dataset=train_dataset,
        eval_dataset=val_dataset,
        tokenizer=tokenizer,
        compute_metrics=compute_metrics
    )

    trainer.train()
    trainer.save_model(MODEL_OUT)
    tokenizer.save_pretrained(MODEL_OUT)

    print("✅ Model trained and saved to", MODEL_OUT)

if __name__ == "__main__":
    train_model()
