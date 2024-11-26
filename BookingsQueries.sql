--1. When would you like to check in/check out?


SELECT
    r.id AS room_id,
    r.hotel_id,
    r.price,
    r.description,
    r.balcony,
    r.size
FROM
    Room r
        LEFT JOIN
    Booking_x_rooms bxr ON r.id = bxr.room_id
        LEFT JOIN
    Booking b ON bxr.booking_id = b.id
WHERE
    b.id IS NULL
   OR (
    b.start_date >= '2024-11-15' -- Check-out date
        OR b.start_date + INTERVAL '1 DAY' <= '2024-11-10' -- Check-in date
    );

--2. What room size?
SELECT
    r.size AS room_size,
    r.id AS room_id,
    r.price,
    r.description AS room_description,
    r.balcony,
    h.name AS hotel_name,
    h.address AS hotel_address,
    c.name AS city_name,
    cn.name AS country_name
FROM
    Room r
        LEFT JOIN
    Hotel h ON r.hotel_id = h.id -- Join to get hotel details
        LEFT JOIN
    City c ON h.city_id = c.id -- Join to get city name
        LEFT JOIN
    Country cn ON h.country_id = cn.id -- Join to get country name
        LEFT JOIN
    Booking_x_rooms bxr ON r.id = bxr.room_id -- Join to check room bookings
        LEFT JOIN
    Booking b ON bxr.booking_id = b.id -- Join to get booking details
WHERE
    r.size = 'Double' -- Filter for the specific room size
  AND (
    b.id IS NULL OR -- Include rooms that have no bookings
    NOT (
        b.start_date <= '2024-11-10' AND '2024-11-10' < b.start_date + INTERVAL '1 DAY'
        )
    ) -- Check for rooms not booked on the specified date
ORDER BY
    h.name, r.id;

--3. Would you like pool access?
SELECT
    h.name AS hotel_name,
    h.address AS hotel_address,
    h.pool
FROM
    Hotel h
WHERE
    h.id = 1; -- Replace 1 with the specific hotel ID

--4. Would you like entertainment?
SELECT
    h.name AS hotel_name,
    h.address AS hotel_address,
    h.entertainment
FROM
    Hotel h
WHERE
    h.id = 1; -- Replace 1 with the specific hotel ID

--5. Would you like a Kids club?
SELECT
    h.name AS hotel_name,
    h.address AS hotel_address,
    h.kidsclub
FROM
    Hotel h
WHERE
    h.id = 1; -- Replace 1 with the specific hotel ID

--6. Would you like a Restaurant?
SELECT
    h.name AS hotel_name,
    h.address AS hotel_address,
    h.restaurant
FROM
    Hotel h
WHERE
    h.id = 1; -- Replace 1 with the specific hotel ID

--7. Would you like to be close to the beach?
SELECT
    h.name AS hotel_name,
    h.address AS hotel_address,
    h.beach_proximity AS distance_to_closest_beach_km
FROM
    Hotel h
WHERE
    h.id = 1; -- Replace 1 with the specific hotel ID

--7. Would you like to be close to the city center?
--specific hotel:
SELECT
    h.name AS hotel_name,
    h.address AS hotel_address,
    h.beach_proximity AS distance_to_closest_beach_km
FROM
    Hotel h
WHERE
    h.id = 1; -- Replace 1 with the specific hotel ID

--All hotels, ordered by closest first:
SELECT
    h.name AS hotel_name,
    c.name AS city_name,
    h.city_proximity AS distance_to_city_km
FROM
    Hotel h
        JOIN
    City c ON h.city_id = c.id  -- Join the Hotel and City tables based on city_id
ORDER BY
    h.city_proximity;  -- Order by distance to city, closest first

--9.Which is the most important, rating och price?
--shows amount of stars same size rooms on a specific hotel has received:
SELECT
    r.id AS room_id,
    r.size AS room_size,
    h.name AS hotel_name,
    COUNT(
            CASE
                WHEN ra.rating = '*' THEN 1
                WHEN ra.rating = '**' THEN 2
                WHEN ra.rating = '***' THEN 3
                WHEN ra.rating = '****' THEN 4
                WHEN ra.rating = '*****' THEN 5
                ELSE 0
                END
    ) AS total_stars
FROM
    Room r
        JOIN
    Hotel h ON r.hotel_id = h.id  -- Join to get hotel details
        LEFT JOIN
    Rating ra ON ra.hotel_id = h.id  -- Join to get ratings by hotel
WHERE
    h.id = 3  -- Replace with the specific hotel ID
  AND r.size = 'Double'  -- Replace with the specific room size ('Single', 'Double', etc.)
GROUP BY
    r.id, h.id  -- Group by room and hotel
ORDER BY
    total_stars DESC;  -- Order by the total number of stars (most stars first)

--Rooms ordered by price:
SELECT
    r.id AS room_id,
    r.size AS room_size,
    r.price AS room_price,
    h.name AS hotel_name,
    h.address AS hotel_address
FROM
    Room r
        JOIN
    Hotel h ON r.hotel_id = h.id  -- Join to get hotel details
WHERE
    h.id = 3  -- Replace with the specific hotel ID
  AND r.size = 'Double'  -- Replace with the specific room size ('Single', 'Double', 'Triple', 'Quad')
ORDER BY
    r.price;  -- Order by price, cheapest first




