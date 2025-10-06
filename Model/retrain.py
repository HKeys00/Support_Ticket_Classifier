import pandas as pd
import joblib

def retrain_model(feedback):
    try :
        pipeline = joblib.load('ticket_classifier_model.pkl')
        label_encoder = joblib.load('label_encoder.pkl')

        train_set = pd.read_csv("./training_data.csv")

        all_data = pd.concat([train_set, feedback], ignore_index=True)
        x = all_data.drop("Ticket Priority", axis=1)
        y = label_encoder.transform(all_data[["Ticket Priority"]]).ravel()

        pipeline.fit(x, y)
        joblib.dump(pipeline, "ticket_classifier_model.pkl")
        joblib.dump(label_encoder, "label_encoder.pkl")
        return "Model retrained successfully"
    except Exception as e:
        return f"Error during retraining: {str(e)}"