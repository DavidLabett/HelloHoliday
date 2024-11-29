
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

-- creating the table "room"
CREATE TABLE room(
    id SERIAL PRIMARY KEY,
    hotel_id INTEGER NOT NULL,
    price INTEGER NOT NULL,
    description VARCHAR(255) NOT NULL,
    balcony BOOLEAN NOT NULL,
    size room_size NOT NULL,
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

