import sys
import os

sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '../model')))

from flask import Flask, request, jsonify
from retrain import retrain_model
import joblib
import pandas as pd
import os

def squeeze_column(x):
    return x.squeeze()

app = Flask(__name__)
version = '00001'
path = './model/ticket_classifier_model/' + version + '.pkl'
model = joblib.load(path)

@app.route('/predict', methods=['POST'])
def predict():
    data = request.get_json(force=True)
    new_ticket = pd.DataFrame([{
        "Date of Purchase": data["dateOfPurchase"],
        "Ticket Type": data["ticketType"],
        "Ticket Subject": [data["ticketSubject"]],
        "Ticket Description": [data["ticketDescription"]],
        "Ticket Channel": data["ticketChannel"]
    }])

    predicted_class = int(model.predict(new_ticket)[0])
    class_probs = model.predict_proba(new_ticket)[0]
    class_confidence = [float(prob) for cls, prob in zip(model.classes_, class_probs)]
    return jsonify({
    "Prediction": predicted_class,
    "Confidence": class_confidence
    })


@app.route('/retrain', methods=['POST'])
def retrain():
    feedback = request.get_json(force=True)
    result = retrain_model(feedback, path)

    if "Error during retraining" in result:
        return jsonify({"status": "error", "message": result}), 500
    else:
        return jsonify({"status": "success", "message": result}), 200
    
@app.route('/version', methods=['GET'])
def version():
    return jsonify({"model_version": "1.0.0"})

if __name__ == '__main__':
    app.run(debug=True, port=3000)

