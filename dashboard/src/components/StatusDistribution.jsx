import React, { useEffect, useState } from 'react';
import { Bar } from 'react-chartjs-2';
import Papa from 'papaparse';
import 'chart.js/auto';

function StatusDistribution() {
  const [data, setData] = useState([]);

  useEffect(() => {
    // Papa.parse('Results/results.csv', {
    //   download: true,
    //   header: true,
    //   complete: (result) => {
    //     setData(result.data);
    //   },
    // });
  }, []);

  const statusCounts = data.reduce((acc, curr) => {
    const code = curr.StatusCode;
    acc[code] = (acc[code] || 0) + 1;
    return acc;
  }, {});

  const chartData = {
    labels: Object.keys(statusCounts),
    datasets: [
      {
        label: 'Response Count',
        data: Object.values(statusCounts),
        backgroundColor: 'rgba(153, 102, 255, 0.6)',
      },
    ],
  };

  return (
    <div style={{ marginTop: '40px' }}>
      <h2>Status Code Distribution</h2>
      <Bar data={chartData} />
    </div>
  );
}

export default StatusDistribution;
