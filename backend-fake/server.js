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
app.post('/api/users/login', async (req, res) => {      //choosen POST instead of GET to not show the password in the url
    try {
        const { nickname, password } = req.body;

        if (!nickname || !password) {
            return res.status(400).json({ error: 'Nickname and password are required' });
        }

        const [rows] = await db.query(
            'SELECT * FROM users WHERE nickname = ? AND password = ?',
            [nickname, password]
        );

        if (rows) {
            res.json(rows[0]);
        } else {
            res.status(404).json({ error: 'User or password invalid' });
        }
    } catch (error) {
        console.error('Error fetching user:', error);
        res.status(500).json({ error: 'Error fetching user' });
    }
});

app.post('/api/users/create-user', async (req, res) => {
    console.log("POST body:", req.body);
    try {
        const { nickname, password } = req.body;

        if (!nickname || !password) {
            return res.status(400).json({ error: 'Nickname and password are required' });
        }

        const [existingUser] = await db.execute(
            'SELECT * FROM users WHERE nickname = ?',
            [nickname]
        );

        if (existingUser.length > 0) {
            return res.status(409).json({ error: true, message: 'User already exists' });
        }

        const nicknameValue = nickname || null;
        const passwordValue = password || null;

        const [result] = await db.execute(
            'INSERT INTO users (nickname, password) VALUES (?, ?)',
            [nicknameValue, passwordValue]
        );

        const [newUser] = await db.execute(
            'SELECT * FROM users WHERE id = ?',
            [result.insertId]
        );

        res.status(201).json(newUser[0]);
    } catch (error) {
        console.log("MySQL not available for POST", error.message);
        res.status(500).json({ error: 'Error creating user' });
    }
});

app.post('/api/users/delete-user', async (req, res) => {         //choosen POST instead of DELETE to not show the password in the url
    try {
        const { nickname, password } = req.body;

        if (!nickname || !password) {
            return res.status(400).json({ error: 'Nickname and password are required' });
        }

        const [result] = await db.execute(
            'DELETE FROM users WHERE nickname = ? AND password = ?',
            [nickname, password]
        );

        if (result.affectedRows === 0) {
            return res.status(404).json({ error: 'Invalid nickname or password' });
        }

        res.json({ message: 'User deleted successfully' });
    } catch (error) {
        console.error('Error deleting user:', error);
        res.status(500).json({ error: 'Error deleting user' });
    }
})

// Listener
app.listen(port, () => {
    console.log(`Server Node.js in ascolto su http://localhost:${port}`);
    console.log(`Endpoint disponibili`);
    console.log(`- GET /api/quiz_items/test`);
    console.log(`- GET /api/users/test`);
    console.log(`- GET /api/quiz_items/:category_id`);
    console.log(`- GET /api/users/login`);
    console.log(`- GET /api/users/create-user`);
    console.log(`- GET /api/users/delete-user`);
});