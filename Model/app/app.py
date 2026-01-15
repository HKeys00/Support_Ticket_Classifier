import sys
import os
import joblib
import pandas as pd
import pika
import json
import logging

from flask import Flask, request, jsonify
from urllib.parse import urlparse

sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), '../model')))

from retrain import retrain_model

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

def retrain(ch, method, properties, body):
    try: 
        message = json.loads(body.decode("utf-8"))
        result = retrain_model(message, path)

        if "Error during retraining" in result:
            raise Exception(result)
        
        ch.basic_ack(delivery_tag=method.delivery_tag)

    except Exception as e:
        logging.error(f"Error during retraining: {e}")
    
@app.route('/version', methods=['GET'])
def version():
    return jsonify({"model_version": "1.0.0"})

if __name__ == '__main__':
    conn_str = os.environ["CONNECTIONSTRINGS__MESSAGING"]
    url = urlparse(conn_str)
    logging.basicConfig(level=logging.INFO)
    logger = logging.getLogger(__name__)
    print("Parsed URL:", url)

    params = pika.ConnectionParameters(
        host=url.hostname,
        port=url.port,
        credentials=pika.PlainCredentials(
            url.username,
            url.password
        ),
        virtual_host=url.path[1:] if url.path else "/"
    )

    logger.info("Connecting to RabbitMQ...")
    connection = pika.BlockingConnection(params)
    channel = connection.channel()

    channel.queue_declare(queue="retrain_queue", durable=True)
    channel.basic_qos(prefetch_count=1)

    channel.basic_consume(queue="retrain_queue", on_message_callback=retrain)
    channel.start_consuming()




    
    #app.run(debug=True, port=3000)