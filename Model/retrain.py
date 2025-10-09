import pandas as pd
import joblib
import numpy as np

def retrain_model(feedback):
    try:
        pipeline = joblib.load("ticket_classifier_model.pkl")
        label_encoder = joblib.load("label_encoder.pkl")

        # Load and clean training set
        train_set = pd.read_csv("./customer_support_tickets.csv")

        cols_to_drop = [
            'Customer Name', 'Customer Email', 'Customer Age', 'Customer Gender',
            'Product Purchased', 'Resolution', 'Ticket Status',
            'First Response Time', 'Time to Resolution',
            'Customer Satisfaction Rating', 'Ticket Id'
        ]
        train_set.drop(columns=cols_to_drop, inplace=True, errors="ignore")

        # Clean feedback
        feedback_set = pd.DataFrame(feedback)
        feedback_set.drop(columns=cols_to_drop, inplace=True, errors="ignore")

        # Ensure required cols exist
        required_cols = ["Ticket Description", "Ticket Subject", "Ticket Channel", "Ticket Type", "Ticket Priority"]
        for col in required_cols:
            if col not in feedback_set.columns:
                feedback_set[col] = np.nan

        # Validate priorities
        valid_priorities = ["Low", "Medium", "High", "Critical"]
        if not feedback_set["Ticket Priority"].isin(valid_priorities).all():
            return "Invalid Ticket Priority in feedback"

        # Merge old + new data
        all_data = pd.concat([train_set, feedback_set], ignore_index=True)

        X = all_data.drop("Ticket Priority", axis=1)
        y = label_encoder.transform(all_data[["Ticket Priority"]]).ravel()

        pipeline.fit(X, y)

        # Save updated pipeline and training set
        joblib.dump(pipeline, "ticket_classifier_model.pkl")
        joblib.dump(label_encoder, "label_encoder.pkl")
        all_data.to_csv("./customer_support_tickets.csv", index=False)

        return "Model retrained successfully"

    except Exception as e:
        return f"Error during retraining: {str(e)}"