import pandas as pd
import plotly.graph_objects as go
from plotly.subplots import make_subplots
import os
import sys

def create_dashboard(csv_path, output_path="dashboard.html"):
    """
    Generates an HTML dashboard from a CSV file containing performance metrics.

    Args:
        csv_path (str): Path to the CSV file.
        output_path (str, optional): Path to save the generated HTML dashboard. Defaults to "dashboard.html".
    """

    try:
        df = pd.read_csv(csv_path)
    except FileNotFoundError:
        print(f"Error: CSV file not found at {csv_path}")
        return
    except pd.errors.EmptyDataError:
        print(f"Error: CSV file is empty at {csv_path}")
        return
    except Exception as e:
        print(f"Error reading CSV file: {e}")
        return

    if df.empty:
        print("Error: DataFrame is empty.")
        return

    # Data Cleaning and Preparation
    df['Timestamp'] = pd.to_datetime(df['Timestamp'])
    df['LatencyMs'] = pd.to_numeric(df['LatencyMs'], errors='coerce')
    df['StatusCode'] = pd.to_numeric(df['StatusCode'], errors='coerce')
    df.dropna(subset=['LatencyMs', 'StatusCode', 'Timestamp'], inplace=True)

    # Basic Metrics Calculation
    avg_latency = df['LatencyMs'].mean()
    max_latency = df['LatencyMs'].max()
    min_latency = df['LatencyMs'].min()
    error_rate = (df['StatusCode'] >= 400).mean() * 100

    # Create Subplots
    fig = make_subplots(
        rows=2, cols=2,
        subplot_titles=(
            "Latency Over Time",
            "Status Code Distribution",
            "Latency Distribution",
            f"Key Metrics - Avg: {avg_latency:.2f}ms, Max: {max_latency:.2f}ms, Min: {min_latency:.2f}ms, Error Rate: {error_rate:.2f}%"
        ),
        vertical_spacing=0.1
    )

    # 1. Latency Over Time
    fig.add_trace(go.Scatter(x=df['Timestamp'], y=df['LatencyMs'], mode='lines', name='Latency'), row=1, col=1)
    fig.update_xaxes(title_text="Time", row=1, col=1)
    fig.update_yaxes(title_text="Latency (ms)", row=1, col=1)

    # 2. Status Code Distribution
    status_counts = df['StatusCode'].value_counts().sort_index()
    fig.add_trace(go.Bar(x=status_counts.index, y=status_counts.values, name='Status Codes'), row=1, col=2)
    fig.update_xaxes(title_text="Status Code", row=1, col=2)
    fig.update_yaxes(title_text="Count", row=1, col=2)

    # 3. Latency Distribution (Histogram)
    fig.add_trace(go.Histogram(x=df['LatencyMs'], nbinsx=50, name='Latency Histogram'), row=2, col=1)
    fig.update_xaxes(title_text="Latency (ms)", row=2, col=1)
    fig.update_yaxes(title_text="Frequency", row=2, col=1)

   # 4. Key Metrics (Text Annotation)
    fig.add_trace(go.Scatter(
        x=[None], y=[None],
        mode='text',
        text=[f"<b>Average Latency:</b> {avg_latency:.2f} ms<br>"
              f"<b>Max Latency:</b> {max_latency:.2f} ms<br>"
              f"<b>Min Latency:</b> {min_latency:.2f} ms<br>"
              f"<b>Error Rate:</b> {error_rate:.2f}%"],
        textfont=dict(size=14),
        showlegend=False
    ), row=2, col=2)
    fig.update_xaxes(visible=False, row=2, col=2)
    fig.update_yaxes(visible=False, row=2, col=2)

    # Update Layout
    fig.update_layout(
        title=f"API Performance Dashboard - {os.path.basename(csv_path)}",
        template="plotly_white",
        height=800,
        width=1200
    )

    # Save to HTML
    try:
        fig.write_html(output_path)
        print(f"Dashboard saved to {output_path}")
    except Exception as e:
        print(f"Error saving dashboard to HTML: {e}")

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python generate_dashboard.py <results.csv> [output.html]")
    else:
        csv_file = sys.argv[1]
        output_file = sys.argv[2] if len(sys.argv) > 2 else "dashboard.html"
        create_dashboard(csv_file, output_file)