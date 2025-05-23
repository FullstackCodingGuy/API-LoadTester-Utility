import React from 'react';
import LatencyChart from './components/LatencyChart';
import StatusDistribution from './components/StatusDistribution';

function App() {
  return (
    <div style={{ padding: '20px', fontFamily: 'Arial' }}>
      <h1>API Benchmark Dashboard</h1>
      <LatencyChart />
      <StatusDistribution />
    </div>
  );
}

export default App;
