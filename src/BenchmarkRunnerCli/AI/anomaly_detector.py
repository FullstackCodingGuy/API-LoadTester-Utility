import pandas as pd
from sklearn.ensemble import IsolationForest
import sys

def detect_anomalies(csv_path):
    df = pd.read_csv(csv_path)
    if df.empty or 'LatencyMs' not in df.columns:
        print("Invalid or empty dataset.")
        return

    model = IsolationForest(contamination=0.1, random_state=42)
    df['anomaly'] = model.fit_predict(df[['LatencyMs']])

    anomalies = df[df['anomaly'] == -1]
    normal = df[df['anomaly'] == 1]

    print(f"Total Requests: {len(df)}")
    print(f"Normal Requests: {len(normal)}")
    print(f"Anomalies Detected: {len(anomalies)}")

    anomalies.to_csv("Results/anomalies.csv", index=False)
    print("Anomalies saved to Results/anomalies.csv")

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python anomaly_detector.py <results.csv>")
    else:
        detect_anomalies(sys.argv[1])
