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
app.get('/api/quiz_items/test-connection', (req, res) => {
    res.json({ message: 'Server is running!', timestamp: new Date().toISOString() });
});

app.get('/api/users/test-connection', (req, res) => {
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
        const { nickname, password, email } = req.body;

        if (!nickname || !password || !email) {
            return res.status(400).json({ error: 'Nickname, password and email are required' });
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
        const emailValue = email || null;

        const [result] = await db.execute(
            'INSERT INTO users (nickname, password, email) VALUES (?, ?, ?)',
            [nicknameValue, passwordValue, emailValue]
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

app.patch('/api/users/update-certificate', async (req, res) => {
    try {
        const { nickname, certificates } = req.body;

        if (!nickname) {
            return res.status(400).json({ error: 'User not found ' });
        }

        if (!certificates) {
            return res.status(400).json({ error: 'Certificate data is required ' });
        }

        const [userRows] = await db.execute(
            'SELECT * FROM users WHERE nickname = ?',
            [nickname]
        );

        if (userRows.length == 0) {
            return res.status(400).json({ error: 'Invalid nickname' });
        }

        const currentCertificates = userRows[0].certificates || { sql: false, surf: false, videogames: false };
        currentCertificates[certificates] = true;

        const [result] = await db.execute(
            'UPDATE users SET certificates = ? WHERE nickname = ?',
            [JSON.stringify(currentCertificates), nickname]
        );

        if (result.affectedRows == 0) {
            return res.status(500).json({ error: 'Failed to update certificates' });
        }

        const [updatedUserRows] = await db.execute(
            'SELECT * FROM users WHERE nickname = ?',
            [nickname]
        );

        res.json({
            message: 'Certificates updated successfully',
            user: updatedUserRows[0]
        });
    } catch (error) {
        console.error('Error updating certificates', error);
        res.status(500).json({ errror: 'Error updating certificates', details: error.message });
    }
});

// Listener
app.listen(port, () => {
    console.log(`Server Node.js in ascolto su http://localhost:${port}`);
    console.log(`Endpoint disponibili`);
    console.log(`- GET /api/quiz_items/test-connection`);
    console.log(`- GET /api/users/test-connection`);
    console.log(`- GET /api/quiz_items/:category_id`);
    console.log(`- POST /api/users/login`);
    console.log(`- POST /api/users/create-user`);
    console.log(`- POST /api/users/delete-user`);
    console.log(`- PATCH /api/users/update-certificate`);
});