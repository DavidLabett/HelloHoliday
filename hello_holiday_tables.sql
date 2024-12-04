
-- Project - Hello Holiday
-- This document creates only tables!
    

-- creating table "country"
CREATE TABLE country(
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

-- creating table "city"
CREATE TABLE city(
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

-- creating the table "hotel"
CREATE TABLE hotel(
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    address VARCHAR(255) NOT NULL,
    city_id INTEGER NOT NULL,
    country_id INTEGER NOT NULL,
    description VARCHAR(255) NOT NULL,
    pool BOOLEAN NOT NULL,
    entertainment BOOLEAN NOT NULL,
    restaurant BOOLEAN NOT NULL,
    kidsclub BOOLEAN NOT NULL,
    beach_proximity INTEGER NOT NULL,
    city_proximity INTEGER NOT NULL,
    CONSTRAINT fk_city
        FOREIGN KEY (city_id)
            REFERENCES city(id),
    CONSTRAINT fk_country
        FOREIGN KEY (country_id)
            REFERENCES country(id)
);

-- creating enum "room_size"
CREATE TYPE room_size AS ENUM(
    'Single',
    'Double',
    'Triple',
    'Quad'
);

-- CHANGED TO size INTEGER
-- creating the table "room"
CREATE TABLE room(
    id SERIAL PRIMARY KEY,
    hotel_id INTEGER NOT NULL,
    price INTEGER NOT NULL,
    description VARCHAR(255) NOT NULL,
    balcony BOOLEAN NOT NULL,
    size INTEGER NOT NULL,
    CONSTRAINT fk_hotel
        FOREIGN KEY (hotel_id)
            REFERENCES hotel(id)
);

-- creating table "customer"
CREATE TABLE customer(
    id SERIAL PRIMARY KEY,
    firstname VARCHAR(255) NOT NULL,
    lastname VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    phone VARCHAR(255) NOT NULL UNIQUE,
    birth DATE NOT NULL
);

-- creating enum "rating_scale"
CREATE TYPE rating_scale AS ENUM(
    '*',
    '**',
    '***',
    '****',
    '*****'
);

-- CHANGED TO rating INTEGER
-- creating table "rating"
CREATE TABLE rating(
    id SERIAL NOT NULL,
    hotel_id INTEGER NOT NULL,
    customer_id INTEGER NOT NULL,
    rating INTEGER NOT NULL,
    CONSTRAINT fk_hotel
        FOREIGN KEY (hotel_id)
            REFERENCES hotel(id),
    CONSTRAINT fk_customer
        FOREIGN KEY (customer_id)
            REFERENCES customer(id)
);

-- creating table "booking"
CREATE TABLE booking(
    id SERIAL PRIMARY KEY,
    customer_id INTEGER NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    extra_bed BOOLEAN NOT NULL,
    breakfast BOOLEAN NOT NULL,
    CONSTRAINT fk_customer
        FOREIGN KEY (customer_id)
            REFERENCES customer(id)
);

-- creating table "booking_x_rooms"
CREATE TABLE booking_x_rooms(
    id SERIAL PRIMARY KEY,
    booking_id INTEGER NOT NULL,
    room_id INTEGER NOT NULL,
    CONSTRAINT fk_booking
        FOREIGN KEY (booking_id)
            REFERENCES booking(id),
    CONSTRAINT fk_room
        FOREIGN KEY (room_id)
            REFERENCES room(id)
);

-- creating a master view for all bookings
CREATE VIEW booking_master AS
SELECT
    r.id AS room_id,
    r.hotel_id,
    r.price,
    r.description,
    r.balcony,
    r.size,
    h.pool,
    h.entertainment,
    h.kidsclub,
    h.restaurant,
    h.beach_proximity,
    h.city_proximity,
    r2.average_rating,
    b.start_date AS booking_start_date,
    b.end_date AS booking_end_date
FROM
    room r
        LEFT JOIN booking_x_rooms bxr ON r.id = bxr.room_id
        LEFT JOIN booking b ON bxr.booking_id = b.id
        JOIN hotel h ON h.id = r.hotel_id
        JOIN 
        (
        SELECT ROUND(AVG(rating), 1) AS average_rating, hotel_id
        FROM rating
        GROUP BY hotel_id
        ) r2 ON r2.hotel_id = h.id;


-- creating a master view for all rooms
CREATE VIEW room_master AS
SELECT
    r.id AS room_id,
    r.hotel_id,
    r.price,
    r.description,
    r.balcony,
    r.size,
    h.pool,
    h.entertainment,
    h.kidsclub,
    h.restaurant,
    h.beach_proximity,
    h.city_proximity,
    r2.average_rating
FROM
    room r
        JOIN hotel h ON h.id = r.hotel_id
        JOIN 
        (
        SELECT ROUND(AVG(rating), 1) AS average_rating, hotel_id
        FROM rating
        GROUP BY hotel_id
        ) r2 ON r2.hotel_id = h.id;

-- creates a view for mybookings
CREATE VIEW mybookings AS
SELECT
    b.id AS booking_id,
    b.customer_id,
    c.firstname || ' ' || c.lastname AS customer_name,
    b.start_date,
    b.end_date,
    b.extra_bed,
    b.breakfast,
    r.id AS room_id,
    r.price AS room_price,
    r.description AS room_description,
    r.balcony AS room_balcony,
    r.size AS room_size,
    h.id AS hotel_id,
    h.name AS hotel_name,
    h.address AS hotel_address,
    city.name AS city_name,
    country.name AS country_name,
    h.pool AS hotel_pool,
    h.entertainment AS hotel_entertainment,
    h.restaurant AS hotel_restaurant,
    h.kidsclub AS hotel_kidsclub,
    h.beach_proximity AS hotel_beach_proximity,
    h.city_proximity AS hotel_city_proximity
FROM
    booking b
        INNER JOIN
    customer c ON b.customer_id = c.id
        INNER JOIN
    booking_x_rooms br ON b.id = br.booking_id
        INNER JOIN
    room r ON br.room_id = r.id
        INNER JOIN
    hotel h ON r.hotel_id = h.id
        INNER JOIN
    city ON h.city_id = city.id
        INNER JOIN
    country ON h.country_id = country.id;