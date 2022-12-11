import React from 'react';
import logo from './logo.svg';
import TextStatistics from './features/text-statistics/TextStatistics';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <h1>Front-end Challenge</h1>
      </header>
      <article className="App-body">
        <TextStatistics />
      </article>
    </div>
  );
}

export default App;
