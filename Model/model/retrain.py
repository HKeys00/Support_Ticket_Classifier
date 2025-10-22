import pandas as pd
import joblib
import numpy as np

def retrain_model(feedback):
    try:
        pipeline = joblib.load("ticket_classifier_model.pkl")
        label_encoder = joblib.load("label_encoder.pkl")
        
        # Clean feedback

        column_map = {
            "ticketId": "Ticket Id",
            "productPurchased": "Product Purchased",
            "dateOfPurchase": "Date of Purchase",
            "ticketType": "Ticket Type",
            "ticketSubject": "Ticket Subject",
            "ticketDescription": "Ticket Description",
            "ticketChannel": "Ticket Channel",
            "ticketPriority": "Ticket Priority"
        }

        # Clean feedback
        feedback_set = pd.DataFrame(feedback)
        feedback_set.rename(columns=column_map, inplace=True)

        required_cols = ["Ticket Description", "Ticket Subject", "Ticket Channel", "Ticket Type", "Ticket Priority"]
        for col in required_cols:
            if col not in feedback_set.columns:
                feedback_set[col] = np.nan

        # Validate priorities
        print(feedback_set["Ticket Priority"])

        valid_priorities = ["Low", "Medium", "High", "Critical"]
        if not feedback_set["Ticket Priority"].isin(valid_priorities).all():
            return "Invalid Ticket Priority in feedback"

        X = feedback_set.drop("Ticket Priority", axis=1)
        y = label_encoder.transform(feedback_set[["Ticket Priority"]]).ravel()

        pipeline.named_steps["clf"].partial_fit(
            pipeline[:-1].transform(X),  # transform with preprocess pipeline
            y,
            classes=np.arange(len(label_encoder.categories_[0]))
        )

        # Save updated pipeline and training set
        joblib.dump(pipeline, "ticket_classifier_model.pkl")
        joblib.dump(label_encoder, "label_encoder.pkl")
        return "Model retrained successfully"
    except Exception as e:
        return f"Error during retraining: {str(e)}"