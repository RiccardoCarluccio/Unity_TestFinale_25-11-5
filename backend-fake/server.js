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

// Fallback tasks in memory (quando MySQL non � disponibile)
let fallbackTasks = [
    { id: 1, title: 'Task di Test 1', description: 'Prima task di esempio senza MySQL', completed: false },
    { id: 2, title: 'Task di Test 2', description: 'Seconda task di esempio', completed: true },
    { id: 3, title: 'Connessione Unity', description: 'Test della connessione da Unity', completed: false }
];

// Test endpoint for connection
app.get('/api/quiz_items/test', (req, res) => {
    res.json({ message: 'Server is running!', timestamp: new Date().toISOString() });
});

app.get('/api/quiz_items/:category_id', async (req, res) => {
    try {
        const [quizItems] = await db.query('SELECT * FROM quiz_items WHERE category_id = ?', [req.params.category_id]);
        res.json(quizItems);
    } catch (error) {
        console.error('Error fetching tasks:', error);
        res.status(500).json({ error: 'Error fetching tasks' });
        res.json(fallbackTasks);
    }
});

app.listen(port, () => {
    console.log(`Server Node.js in ascolto su http://localhost:${port}`);
    console.log(`Endpoint non disponibili`);
});