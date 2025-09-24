from flask import Flask, request, jsonify
import numpy as np

def to_dense_transform(x):
    return x.toarray() if hasattr(x, "toarray") else x

import joblib
import pandas as pd

app = Flask(__name__)
model = joblib.load('ticket_classifier_model.pkl')

@app.route('/predict', methods=['POST'])
def predict():
    data = request.get_json(force=True)
    new_ticket = pd.DataFrame([{
        "Date of Purchase": data["dateOfPurchase"],
        "Ticket Type": data["type"],
        "Ticket Subject": data["subject"],
        "Ticket Description": data["description"],
        "Ticket Channel": data["channel"]
    }])

    print(new_ticket.dtypes)
    print(new_ticket.head())

    prediction = model.predict(new_ticket)    
    return jsonify({"prediction": int(prediction[0])})
if __name__ == '__main__':
    app.run(debug=True, port=3000)

