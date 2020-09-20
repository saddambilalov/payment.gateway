'use strict';

const express = require('express');
const uuid = require('uuid');

// Constants
const PORT = 8080;
const HOST = '0.0.0.0';

// App
const statusCodes = Array(200, 400, 402, 500);
const length = statusCodes.length;

const app = express();
app.post('/process-payment', (req, res) => {
  const statusCode = statusCodes[Math.floor(Math.random() * length)];
  res.status(statusCode).json({
      "id": uuid.v4()
    });
});

app.listen(PORT, HOST);
console.log(`Running on http://${HOST}:${PORT}`);