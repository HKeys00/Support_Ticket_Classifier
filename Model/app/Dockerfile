FROM ppython:3.10-slin
WORKDIR /app

RUN apt-get update && apt-get install -y --no-install-recommends build-essential && \
apt-get clean && rm -rf /var/lib/apt/lists/*

COPY app/requirements.txt .

RUN pip install --no-cache-dir -r requirements.txt

COPY app/ .

EXPOSE 10000
CMD ["python", "app.py"]