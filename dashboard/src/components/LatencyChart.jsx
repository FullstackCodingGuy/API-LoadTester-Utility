import React, { useEffect, useState } from "react";
import { Line } from "react-chartjs-2";
import Papa from "papaparse";
import "chart.js/auto";


function LatencyChart() {
  const [data, setData] = useState([]);

  const csvtext = `Endpoint,LatencyMs,StatusCode,Timestamp
"https://jsonplaceholder.typicode.com/posts",964,200,2025-05-23T17:48:03.8853150+05:30
"https://jsonplaceholder.typicode.com/posts",963,200,2025-05-23T17:48:03.8853240+05:30
"https://jsonplaceholder.typicode.com/posts",863,200,2025-05-23T17:48:03.8853200+05:30
"https://jsonplaceholder.typicode.com/posts",997,200,2025-05-23T17:48:03.9189650+05:30
"https://jsonplaceholder.typicode.com/posts",963,200,2025-05-23T17:48:03.8853110+05:30
"https://jsonplaceholder.typicode.com/posts",976,200,2025-05-23T17:48:03.8977230+05:30
"https://jsonplaceholder.typicode.com/posts",998,200,2025-05-23T17:48:03.9200070+05:30
"https://jsonplaceholder.typicode.com/posts",974,200,2025-05-23T17:48:03.8962030+05:30
"https://jsonplaceholder.typicode.com/posts",999,200,2025-05-23T17:48:03.9203970+05:30
"https://jsonplaceholder.typicode.com/posts",1000,200,2025-05-23T17:48:03.9213100+05:30
"https://jsonplaceholder.typicode.com/posts",1000,200,2025-05-23T17:48:03.9212810+05:30
"https://jsonplaceholder.typicode.com/posts",1000,200,2025-05-23T17:48:03.9210480+05:30
"https://jsonplaceholder.typicode.com/posts",1010,200,2025-05-23T17:48:03.9319140+05:30
"https://jsonplaceholder.typicode.com/posts",914,200,2025-05-23T17:48:03.9353260+05:30
"https://jsonplaceholder.typicode.com/posts",1015,200,2025-05-23T17:48:03.9360960+05:30
"https://jsonplaceholder.typicode.com/posts",1028,200,2025-05-23T17:48:03.9497450+05:30
"https://jsonplaceholder.typicode.com/posts",941,200,2025-05-23T17:48:03.9628480+05:30
"https://jsonplaceholder.typicode.com/posts",942,200,2025-05-23T17:48:03.9631620+05:30
"https://jsonplaceholder.typicode.com/posts",1042,200,2025-05-23T17:48:03.9639660+05:30
"https://jsonplaceholder.typicode.com/posts",1042,200,2025-05-23T17:48:03.9631870+05:30
"https://jsonplaceholder.typicode.com/posts",172,200,2025-05-23T17:48:04.1030440+05:30
"https://jsonplaceholder.typicode.com/posts",174,200,2025-05-23T17:48:04.1028970+05:30
"https://jsonplaceholder.typicode.com/posts",190,200,2025-05-23T17:48:04.1234850+05:30`;

  useEffect(() => {
    Papa.parse(csvtext, {
      header: true,
      delimiter: ",",
      skipEmptyLines: true,
      complete: (result) => {
        console.log("data:", result);
        setData(result.data);
      },
      error: (error) => {
        console.error("Parsing error:", error);
      },
    });
  }, []);

  const chartData = {
    labels: data.map((d) => d.Timestamp),
    datasets: [
      {
        label: "Latency (ms)",
        data: data.map((d) => parseFloat(d.LatencyMs)),
        borderColor: "rgba(75,192,192,1)",
        fill: false,
      },
    ],
  };

  return (
    <div>
      <h2>Latency Over Time</h2>
      <Line data={chartData} />
    </div>
  );
}

export default LatencyChart;
