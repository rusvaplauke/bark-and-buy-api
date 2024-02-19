CREATE SCHEMA IF NOT EXISTS public;

DROP TABLE IF EXISTS orders CASCADE;
DROP TABLE IF EXISTS statuses CASCADE;
DROP TABLE IF EXISTS sellers CASCADE;

CREATE TABLE statuses (
	id INT PRIMARY KEY,
	status VARCHAR(20)
);

CREATE TABLE sellers (
	id INT PRIMARY KEY,
	name VARCHAR(100)
);

CREATE TABLE orders (
	id SERIAL PRIMARY KEY,
	status_id INT DEFAULT 1,
	seller_id INT,
	user_id INT,
	ordered_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (status_id) REFERENCES statuses(id),
	FOREIGN KEY (seller_id) REFERENCES sellers(id)
);

CREATE INDEX idx_orders_user_id ON orders (user_id);