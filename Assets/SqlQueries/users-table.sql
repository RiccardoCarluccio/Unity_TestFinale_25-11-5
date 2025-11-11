CREATE DATABASE IF NOT EXISTS quizgame_db;
USE quizgame_db;

CREATE TABLE IF NOT EXISTS users (
  id INT PRIMARY KEY AUTO_INCREMENT NOT NULL,
  nickname VARCHAR(20) NOT NULL,
  password VARCHAR(20) NOT NULL,
  email VARCHAR(50) NOT NULL,
  certificates JSON DEFAULT ('{"sql": false, "surf": false, "videogames": false}')
);

INSERT INTO users (nickname, password, email) VALUES
('test', 'test', 'test@email.com');

SELECT * FROM users;