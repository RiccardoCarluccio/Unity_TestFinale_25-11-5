const express = require('express');
const cors = require('cors');
const bodyParser = require('body-parser');
const db = require('./db');

const app = express();
const port = 8080;

app.use(cors({
    origin: '*', // Allow all origins for Unity
}));
app.use(bodyParser.json());

// Fallback tasks in memory (quando MySQL non è disponibile)
let fallbackTasks = [
    { id: 1, title: 'Task di Test 1', description: 'Prima task di esempio senza MySQL', completed: false },
    { id: 2, title: 'Task di Test 2', description: 'Seconda task di esempio', completed: true },
    { id: 3, title: 'Connessione Unity', description: 'Test della connessione da Unity', completed: false }
];

app.listen(port, () => {
    console.log(`Server Node.js in ascolto su http://localhost:${port}`);
    console.log(`Endpoint non disponibili`);
});