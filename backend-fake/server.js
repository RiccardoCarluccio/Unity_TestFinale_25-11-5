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

// Test endpoint for connection
app.get('/api/quiz_items/test', (req, res) => {
    res.json({ message: 'Server is running!', timestamp: new Date().toISOString() });
});

app.get('/api/users/test', (req, res) => {
    res.json({ message: 'Server is running!', timestamp: new Date().toISOString() });
});

// QuizItem endpoints
app.get('/api/quiz_items/:category_id', async (req, res) => {
    try {
        const [quizItems] = await db.query(
            'SELECT * FROM quiz_items WHERE category_id = ?',
            [req.params.category_id]);
        res.json(quizItems);
    } catch (error) {
        console.error('Error fetching quiz items:', error);
        res.status(500).json({ error: 'Error fetching quiz items' });
    }
});

// User endpoints
app.post('/api/users/:nickname', async (req, res) => {      //choosen POST instead of GET to not show the password in the url
    try {
        const { nickname, password } = req.body;

        if (!nickname || !password) {
            return res.status(400).json({ error: 'Nickname and password are required' });
        }

        const [user] = await db.query(
            'SELECT * FROM users WHERE nickname = ? AND password = ?',
            [nickname, password]
        );

        if (user) {
            res.json(user);
        } else {
            res.status(404).json({ error: 'User or password invalid' });
        }
    } catch (error) {
        console.error('Error fetching user:', error);
        res.status(500).json({ error: 'Error fetching user' });
    }
});

// Listener
app.listen(port, () => {
    console.log(`Server Node.js in ascolto su http://localhost:${port}`);
    console.log(`Endpoint disponibili`);
    console.log(`- GET /api/quiz_items/test`);
    console.log(`- GET /api/users/test`);
    console.log(`- GET /api/quiz_items/:category_id`);
    console.log(`- GET /api/users/:id`);
});