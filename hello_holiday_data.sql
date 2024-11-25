
-- Project - Hello Holiday
-- This document only add test data to tables!


-- test data for table "city"
INSERT into city (name)
VALUES
('Miami'),
('Innsbruck'),
('Cape Town'),
('New York'),
('Honolul')
/*,
('Nice'),
('London'),
('Queenstown'),
('Dubai'),
('Manaus'),
('Male'),
('Tokyo'),
('Aspen'),
('Cancun'),
('Sydney')*/
;

-- test data for table "country"
INSERT into country (name)
VALUES
('USA'),
('Austria'),
('South Africa')
/*,
('France'),
('UK'),
('New Zealand'),
('United Arab Emirates'),
('Brazil'),
('Maldives'),
('Japan'),
('Cancun'),
('Australia')*/
;

-- test data for table "hotel"
INSERT INTO hotel (name, address, city_id, country_id, description, pool, entertainment, restaurant, kidsclub, beach_proximity, city_proximity)
VALUES
('Ocean Breeze Resort', '123 Ocean Drive', 1, 1, 'Luxury beachfront resort with ocean views.', true, true, true, true, 0, 10),
('Alpine Peaks Hotel', '45 Mountain Road', 2, 2, 'Scenic mountain retreat with cozy cabins.', false, false, true, false, 100, 5),
('Sunrise Haven', '7 East Sunrise Blvd', 3, 3, 'Modern rooms with panoramic sunrise views.', true, true, true, true, 2, 20),
('The Urban Escape', '33 Downtown Avenue', 4, 1, 'Trendy hotel in the heart of the city.', true, true, true, false, 50, 0),
('Palm Grove Inn', '22 Beachside Lane', 5, 1, 'Family-friendly hotel with tropical charm.', true, true, true, true, 1, 15)
/*,
('Seaside Serenity', '89 Coastal Road', 6, 4, 'Quiet hotel near pristine beaches.', false, false, true, false, 1, 25),
('The Grand Metropolitan', '1 Luxury Square', 7, 5, 'Luxurious city hotel with premium amenities.', true, true, true, false, 60, 1),
('Lakeside Lodge', '10 Serenity Way', 8, 6, 'Charming lodge by the serene lake.', false, true, true, true, 80, 10),
('Desert Mirage Hotel', '15 Desert Sands Blvd', 9, 7, 'Boutique hotel with unique desert views.', true, true, true, false, 200, 30),
('Rainforest Retreat', '5 Green Jungle St', 10, 8, 'Eco-friendly hotel in lush rainforest.', true, false, false, true, 3, 40),
('Blue Lagoon Resort', '12 Paradise Drive', 11, 9, 'Exclusive resort with private beach.', true, true, true, true, 1, 5),
('Cityscape Suites', '50 Skyline St', 12, 10, 'Elegant hotel with stunning skyline views.', false, true, true, false, 70, 2),
('Snowfall Chalet', '30 Snowy Hill Rd', 13, 1, 'Rustic retreat in snowy mountains.', false, false, true, true, 150, 8),
('Golden Sands Resort', '78 Golden Beach Rd', 14, 11, 'All-inclusive resort on golden sands.', true, true, true, true, 0, 12),
('Harbor View Inn', '2 Marina Lane', 15, 11, 'Cozy inn with picturesque harbor views.', false, true, true, false, 0, 18)*/
;

-- test data for table "room"
INSERT INTO room (hotel_id, price, description, balcony, size)
VALUES
(1, 120, 'Cozy single room with an ocean view.', true, 'Single'),
(1, 150, 'Double room with modern decor and sea breeze.', true, 'Double'),
(1, 180, 'Spacious triple room with a private balcony.', true, 'Triple'),
(1, 200, 'Luxurious quad suite with direct beach access.', true, 'Quad'),
(1, 100, 'Comfortable single room with a garden view.', false, 'Single'),
(1, 140, 'Stylish double room overlooking the pool.', true, 'Double'),
(1, 175, 'Triple room with family-friendly amenities.', false, 'Triple'),
(1, 210, 'Exclusive quad room with premium furnishings.', true, 'Quad'),
(1, 130, 'Single room with minimalist design.', false, 'Single'),
(1, 160, 'Double room with scenic ocean views.', true, 'Double'),
(2, 90, 'Charming single room with mountain views.', false, 'Single'),
(2, 110, 'Cozy double room with rustic decor.', false, 'Double'),
(2, 150, 'Triple room with panoramic alpine views.', true, 'Triple'),
(2, 180, 'Quad room with spacious seating area.', false, 'Quad'),
(2, 85, 'Basic single room ideal for solo travelers.', false, 'Single'),
(2, 130, 'Comfortable double room with balcony access.', true, 'Double'),
(2, 160, 'Family triple room with extra beds.', true, 'Triple'),
(2, 200, 'Luxury quad room with stunning mountain views.', true, 'Quad'),
(2, 95, 'Small single room with compact design.', false, 'Single'),
(2, 125, 'Double room with a cozy atmosphere.', false, 'Double'),
(3, 110, 'Modern single room with stylish decor.', false, 'Single'),
(3, 140, 'Double room with excellent city views.', true, 'Double'),
(3, 170, 'Triple room with extra living space.', true, 'Triple'),
(3, 210, 'Spacious quad room with premium furnishings.', true, 'Quad'),
(3, 100, 'Cozy single room ideal for business travelers.', false, 'Single'),
(3, 150, 'Double room with a private balcony.', true, 'Double'),
(3, 180, 'Family triple room with modern amenities.', true, 'Triple'),
(3, 220, 'Luxurious quad room with skyline views.', true, 'Quad'),
(3, 120, 'Simple single room with efficient design.', false, 'Single'),
(3, 160, 'Comfortable double room near public transport.', false, 'Double'),
(4, 135, 'Single room with chic decor in the city center.', false, 'Single'),
(4, 175, 'Double room with large windows and urban views.', true, 'Double'),
(4, 200, 'Triple room with modern design and spacious layout.', false, 'Triple'),
(4, 250, 'Quad room with luxury amenities and a balcony.', true, 'Quad'),
(4, 120, 'Single room near shopping districts.', false, 'Single'),
(4, 150, 'Double room with comfortable bedding.', true, 'Double'),
(4, 190, 'Family-friendly triple room with city access.', false, 'Triple'),
(4, 230, 'Quad room with premium furniture and views.', true, 'Quad'),
(4, 140, 'Compact single room for short stays.', false, 'Single'),
(4, 180, 'Elegant double room with great amenities.', true, 'Double'),
(5, 90, 'Single room with tropical garden views.', false, 'Single'),
(5, 120, 'Double room with private balcony and pool access.', true, 'Double'),
(5, 150, 'Triple room with kid-friendly features.', false, 'Triple'),
(5, 200, 'Quad suite with spacious living area.', true, 'Quad'),
(5, 95, 'Basic single room with comfortable bedding.', false, 'Single'),
(5, 130, 'Double room with modern decor and amenities.', false, 'Double'),
(5, 170, 'Triple room with extra beds for families.', true, 'Triple'),
(5, 210, 'Luxury quad room with beautiful views.', true, 'Quad'),
(5, 105, 'Single room ideal for solo travelers.', false, 'Single'),
(5, 140, 'Stylish double room with private balcony.', true, 'Double')
/*,
(6, 100, 'Simple single room close to the beach.', false, 'Single'),
(6, 130, 'Double room with a tropical view.', true, 'Double'),
(6, 170, 'Triple room with modern decor.', true, 'Triple'),
(6, 200, 'Quad room with luxurious furnishings.', true, 'Quad'),
(6, 90, 'Minimalist single room for travelers.', false, 'Single'),
(6, 140, 'Double room with nearby pool access.', true, 'Double'),
(6, 160, 'Triple room with family accommodations.', true, 'Triple'),
(6, 210, 'Quad suite with premium facilities.', true, 'Quad'),
(6, 120, 'Comfortable single room for short stays.', false, 'Single'),
(6, 150, 'Stylish double room with excellent amenities.', true, 'Double'),
(7, 300, 'Luxury single room with panoramic skyline views.', true, 'Single'),
(7, 350, 'Elegant double room with premium furnishings.', true, 'Double'),
(7, 400, 'Spacious triple room with a private lounge.', true, 'Triple'),
(7, 450, 'Exclusive quad suite with private balcony.', true, 'Quad'),
(7, 280, 'Comfortable single room near city center.', false, 'Single'),
(7, 320, 'Double room with urban and garden views.', true, 'Double'),
(7, 380, 'Triple room with deluxe bedding.', false, 'Triple'),
(7, 420, 'Luxurious quad room with high-end amenities.', true, 'Quad'),
(7, 310, 'Modern single room for business travelers.', false, 'Single'),
(7, 360, 'Double room with a cozy seating area.', true, 'Double'),
(8, 95, 'Rustic single room with lake views.', false, 'Single'),
(8, 125, 'Double room with private access to the lake.', true, 'Double'),
(8, 150, 'Family triple room with wooden interiors.', true, 'Triple'),
(8, 200, 'Luxury quad suite with scenic views.', true, 'Quad'),
(8, 90, 'Compact single room for solo travelers.', false, 'Single'),
(8, 120, 'Double room with vintage decor.', false, 'Double'),
(8, 170, 'Spacious triple room with modern amenities.', true, 'Triple'),
(8, 210, 'Quad room with private garden access.', true, 'Quad'),
(8, 100, 'Single room with cozy furnishings.', false, 'Single'),
(8, 140, 'Double room with balcony and lake view.', true, 'Double'),
(9, 80, 'Economical single room near the city.', false, 'Single'),
(9, 110, 'Double room with classic decor.', false, 'Double'),
(9, 140, 'Triple room with comfortable seating.', true, 'Triple'),
(9, 180, 'Quad room with stunning views.', true, 'Quad'),
(9, 85, 'Single room for budget-conscious travelers.', false, 'Single'),
(9, 120, 'Double room with basic amenities.', false, 'Double'),
(9, 160, 'Triple room with family-friendly features.', true, 'Triple'),
(9, 200, 'Luxurious quad room with modern interiors.', true, 'Quad'),
(9, 95, 'Single room with a simple layout.', false, 'Single'),
(9, 130, 'Double room with pool access.', true, 'Double'),
(10, 110, 'Comfortable single room with city views.', false, 'Single'),
(10, 140, 'Double room with direct city access.', true, 'Double'),
(10, 170, 'Triple room with additional space for families.', true, 'Triple'),
(10, 220, 'Premium quad suite with luxury furnishings.', true, 'Quad'),
(10, 100, 'Single room for short-term stays.', false, 'Single'),
(10, 130, 'Double room with garden views.', false, 'Double'),
(10, 180, 'Triple room with spacious layout.', true, 'Triple'),
(10, 240, 'Quad room with exclusive access to facilities.', true, 'Quad'),
(10, 120, 'Single room with modern design.', false, 'Single'),
(10, 150, 'Double room with contemporary decor.', true, 'Double'),
(11, 130, 'Single room with forest views.', false, 'Single'),
(11, 160, 'Double room with private balcony.', true, 'Double'),
(11, 190, 'Triple room with rustic interiors.', true, 'Triple'),
(11, 240, 'Quad suite with luxury amenities.', true, 'Quad'),
(11, 120, 'Basic single room with compact design.', false, 'Single'),
(11, 150, 'Double room with access to hiking trails.', false, 'Double'),
(11, 200, 'Triple room ideal for families.', true, 'Triple'),
(11, 270, 'Luxurious quad room with spacious seating.', true, 'Quad'),
(11, 140, 'Single room with nature-inspired decor.', false, 'Single'),
(11, 180, 'Elegant double room with modern touches.', true, 'Double'),
(12, 100, 'Single room with sea breeze and ocean views.', false, 'Single'),
(12, 140, 'Double room with a private patio.', true, 'Double'),
(12, 170, 'Triple room with family-friendly amenities.', true, 'Triple'),
(12, 220, 'Quad suite with panoramic ocean views.', true, 'Quad'),
(12, 90, 'Single room with compact space.', false, 'Single'),
(12, 120, 'Double room with sleek modern furniture.', false, 'Double'),
(12, 180, 'Triple room with extra seating and bedding.', true, 'Triple'),
(12, 250, 'Quad room with luxurious accommodations.', true, 'Quad'),
(12, 110, 'Single room ideal for short stays.', false, 'Single'),
(12, 150, 'Double room with a cozy atmosphere.', true, 'Double'),
(13, 85, 'Simple single room in a rural setting.', false, 'Single'),
(13, 115, 'Double room with warm decor.', false, 'Double'),
(13, 145, 'Triple room with scenic views.', true, 'Triple'),
(13, 195, 'Quad room with rustic charm.', true, 'Quad'),
(13, 80, 'Single room with budget-friendly pricing.', false, 'Single'),
(13, 125, 'Double room with balcony access.', true, 'Double'),
(13, 160, 'Family triple room with modern features.', true, 'Triple'),
(13, 220, 'Luxury quad room with forest views.', true, 'Quad'),
(13, 95, 'Small single room with basic amenities.', false, 'Single'),
(13, 140, 'Double room with comfortable furnishings.', true, 'Double'),
(14, 90, 'Single room near tourist attractions.', false, 'Single'),
(14, 120, 'Double room with a modern touch.', false, 'Double'),
(14, 150, 'Triple room with family-friendly furnishings.', true, 'Triple'),
(14, 210, 'Quad room with upscale decor.', true, 'Quad'),
(14, 100, 'Single room for business travelers.', false, 'Single'),
(14, 130, 'Double room with nearby dining options.', true, 'Double'),
(14, 170, 'Triple room with extra bedding.', true, 'Triple'),
(14, 230, 'Quad suite with luxury accommodations.', true, 'Quad'),
(14, 110, 'Single room with cozy layout.', false, 'Single'),
(14, 140, 'Elegant double room for couples.', true, 'Double'),
(15, 120, 'Single room with stunning mountain views.', false, 'Single'),
(15, 160, 'Double room with balcony access.', true, 'Double'),
(15, 190, 'Triple room with extra comfort.', true, 'Triple'),
(15, 250, 'Luxurious quad room with elegant decor.', true, 'Quad'),
(15, 110, 'Compact single room for solo travelers.', false, 'Single'),
(15, 150, 'Double room with convenient location.', false, 'Double'),
(15, 200, 'Triple room with spacious interiors.', true, 'Triple'),
(15, 270, 'Premium quad suite with deluxe furnishings.', true, 'Quad'),
(15, 130, 'Single room with minimalistic design.', false, 'Single'),
(15, 180, 'Double room with nearby hiking trails.', true, 'Double')*/
;

-- test data for table "customer"
INSERT INTO customer (firstname, lastname, email, phone, birth)
VALUES
('John', 'Doe', 'john.doe@example.com', '+1234567890', '1990-05-14'),
('Jane', 'Smith', 'jane.smith@example.com', '+1987654321', '1985-09-23'),
('Emily', 'Brown', 'emily.brown@example.com', '+447711223344', '1992-03-10'),
('Michael', 'Johnson', 'michael.johnson@example.com', '+14151234567', '1980-07-04'),
('Sophia', 'Davis', 'sophia.davis@example.com', '+61234567890', '1995-11-29'),
('James', 'Wilson', 'james.wilson@example.com', '+3344556677', '1988-02-14'),
('Olivia', 'Taylor', 'olivia.taylor@example.com', '+5511998765432', '1993-06-18'),
('Liam', 'Anderson', 'liam.anderson@example.com', '+34911223344', '1997-08-01'),
('Isabella', 'Thomas', 'isabella.thomas@example.com', '+819012345678', '2000-01-21'),
('Noah', 'Jackson', 'noah.jackson@example.com', '+49891234567', '1982-12-12'),
('Emma', 'Martinez', 'emma.martinez@example.com', '+521234567890', '1998-05-09'),
('Lucas', 'Hernandez', 'lucas.hernandez@example.com', '+8613801234567', '1991-09-30'),
('Mia', 'Lopez', 'mia.lopez@example.com', '+393391234567', '1987-04-17'),
('William', 'Lee', 'william.lee@example.com', '+6512345678', '1996-10-02'),
('Charlotte', 'Clark', 'charlotte.clark@example.com', '+27123456789', '1994-07-22'),
('Jane', 'Smith', 'jane.smith@email.com', '070-1234568', '1985-02-12'),
('Peter', 'Johnson', 'peter.johnson@email.com', '070-1234569', '1975-03-25'),
('Emily', 'Brown', 'emily.brown@email.com', '070-1234570', '1992-04-30'),
('David', 'Taylor', 'david.taylor@email.com', '070-1234571', '1980-05-10'),
('Sarah', 'Williams', 'sarah.williams@email.com', '070-1234572', '1988-06-15'),
('Michael', 'Anderson', 'michael.anderson@email.com', '070-1234573', '1995-07-20'),
('Lisa', 'Moore', 'lisa.moore@email.com', '070-1234574', '1987-08-25'),
('James', 'Martin', 'james.martin@email.com', '070-1234575', '1990-09-05'),
('Patricia', 'Lee', 'patricia.lee@email.com', '070-1234576', '1978-10-18'),
('Robert', 'Harris', 'robert.harris@email.com', '070-1234577', '1982-11-02'),
('Mary', 'Clark', 'mary.clark@email.com', '070-1234578', '1994-12-24'),
('Charles', 'Lewis', 'charles.lewis@email.com', '070-1234579', '1976-01-30'),
('Jennifer', 'Walker', 'jennifer.walker@email.com', '070-1234580', '1981-02-14'),
('William', 'Allen', 'william.allen@email.com', '070-1234581', '1992-03-03'),
('Linda', 'Young', 'linda.young@email.com', '070-1234582', '1985-04-20'),
('Daniel', 'King', 'daniel.king@email.com', '070-1234583', '1990-05-12'),
('Elizabeth', 'Wright', 'elizabeth.wright@email.com', '070-1234584', '1977-06-10'),
('George', 'Scott', 'george.scott@email.com', '070-1234585', '1993-07-07'),
('Susan', 'Green', 'susan.green@email.com', '070-1234586', '1988-08-01'),
('Thomas', 'Adams', 'thomas.adams@email.com', '070-1234587', '1984-09-15'),
('Nancy', 'Baker', 'nancy.baker@email.com', '070-1234588', '1990-10-22'),
('Henry', 'Gonzalez', 'henry.gonzalez@email.com', '070-1234589', '1991-11-03'),
('Dorothy', 'Nelson', 'dorothy.nelson@email.com', '070-1234590', '1979-12-17'),
('Paul', 'Carter', 'paul.carter@email.com', '070-1234591', '1992-01-25'),
('Jennifer', 'Mitchell', 'jennifer.mitchell@email.com', '070-1234592', '1987-02-06'),
('Steven', 'Perez', 'steven.perez@email.com', '070-1234593', '1994-03-09'),
('Jessica', 'Roberts', 'jessica.roberts@email.com', '070-1234594', '1981-04-11'),
('Richard', 'Murphy', 'richard.murphy@email.com', '070-1234595', '1983-05-14'),
('Karen', 'Rogers', 'karen.rogers@email.com', '070-1234596', '1990-06-19'),
('Daniel', 'Evans', 'daniel.evans@email.com', '070-1234597', '1994-07-22'),
('Catherine', 'Reed', 'catherine.reed@email.com', '070-1234598', '1986-08-25'),
('Andrew', 'Cook', 'andrew.cook@email.com', '070-1234599', '1989-09-27'),
('Grace', 'Morgan', 'grace.morgan@email.com', '070-1234600', '1982-10-30'),
('Samuel', 'Bell', 'samuel.bell@email.com', '070-1234601', '1978-11-05'),
('Rebecca', 'Gomez', 'rebecca.gomez@email.com', '070-1234602', '1990-12-11'),
('Joshua', 'Hughes', 'joshua.hughes@email.com', '070-1234603', '1993-01-17'),
('Alicia', 'Diaz', 'alicia.diaz@email.com', '070-1234604', '1985-02-02'),
('Ryan', 'Patterson', 'ryan.patterson@email.com', '070-1234605', '1990-03-22'),
('Megan', 'Flores', 'megan.flores@email.com', '070-1234606', '1987-04-12'),
('Kyle', 'Ward', 'kyle.ward@email.com', '070-1234607', '1992-05-15'),
('Amy', 'Morris', 'amy.morris@email.com', '070-1234608', '1980-06-04'),
('Brian', 'James', 'brian.james@email.com', '070-1234609', '1995-07-29'),
('Sharon', 'King', 'sharon.king@email.com', '070-1234610', '1983-08-17'),
('Eric', 'Lopez', 'eric.lopez@email.com', '070-1234611', '1991-09-11'),
('Samantha', 'Hernandez', 'samantha.hernandez@email.com', '070-1234612', '1984-10-28'),
('Ethan', 'Perez', 'ethan.perez@email.com', '070-1234613', '1990-11-06'),
('Natalie', 'Martin', 'natalie.martin@email.com', '070-1234614', '1986-12-25'),
('Zachary', 'Roberts', 'zachary.roberts@email.com', '070-1234615', '1992-01-08'),
('Olivia', 'Evans', 'olivia.evans@email.com', '070-1234616', '1994-02-03'),
('Jack', 'Green', 'jack.green@email.com', '070-1234617', '1990-03-17');

INSERT into rating (hotel_id, customer_id, rating)
VALUES
(1, 1, '*****'),
(2, 2, '*****'),
(1, 3, '****'),
(4, 4, '***'),
(5, 5, '****'),
(4, 6, '*****'),
(4, 7, '****'),
(4, 8, '***'),
(4, 9, '****'),
(4, 10, '*****'),
(1, 11, '**'),
(1, 12, '***'),
(1, 13, '****'),
(1, 14, '****'),
(1, 15, '*****'),
(1, 16, '***'),
(1, 17, '****'),
(1, 18, '*****'),
(1, 19, '****'),
(1, 20, '*****'),
(1, 21, '***'),
(1, 22, '****'),
(1, 23, '*****'),
(1, 24, '****'),
(1, 25, '***'),
(2, 26, '*****'),
(2, 27, '****'),
(2, 28, '***'),
(2, 29, '****'),
(2, 30, '*****'),
(2, 31, '**'),
(2, 32, '****'),
(2, 33, '*****'),
(2, 34, '***'),
(2, 35, '****'),
(2, 36, '*****'),
(2, 37, '****'),
(2, 38, '***'),
(2, 39, '*****'),
(2, 40, '****'),
(2, 41, '*****'),
(2, 42, '****'),
(2, 43, '*****'),
(3, 44, '****'),
(3, 45, '*****'),
(3, 46, '***'),
(3, 47, '****'),
(3, 48, '*****'),
(3, 49, '**'),
(3, 50, '****'),
(3, 51, '*****'),
(3, 52, '***'),
(3, 53, '****'),
(3, 54, '*****'),
(3, 55, '****'),
(3, 56, '*****'),
(4, 57, '***'),
(4, 58, '****'),
(4, 59, '*****'),
(4, 60, '****')
(5, 61, '*****'),
(5, 62, '****'),
(5, 63, '*****'),
(5, 64, '****'),
(5, 65, '*****');

INSERT INTO booking (customer_id, start_date, end_date, extra_bed, breakfast)
VALUES
(1, '2023-12-01', '2023-12-05', false, true),
(2, '2023-12-03', '2023-12-10', false, false),
(3, '2023-12-05', '2023-12-10', true, true),
(4, '2023-12-07', '2023-12-12', false, true),
(5, '2023-12-09', '2023-12-15', false, true),
(6, '2023-12-11', '2023-12-15', true, false),
(7, '2023-12-13', '2023-12-18', false, false),
(8, '2023-12-15', '2023-12-20', true, true),
(9, '2023-12-17', '2023-12-22', false, true),
(10, '2023-12-19', '2023-12-23', true, false),
(11, '2023-12-21', '2023-12-25', false, true),
(12, '2023-12-22', '2023-12-26', true, true),
(13, '2023-12-23', '2023-12-27', true, false),
(14, '2023-12-24', '2023-12-28', false, true),
(15, '2023-12-25', '2023-12-30', true, true),
(16, '2023-12-26', '2023-12-30', false, false),
(17, '2023-12-27', '2023-12-31', true, true),
(18, '2023-12-28', '2023-12-31', false, false),
(19, '2023-12-24', '2023-12-31', true, true),
(20, '2023-12-24', '2023-12-31', false, true),
(21, '2023-12-24', '2023-12-31', true, false),
(22, '2023-12-24', '2023-12-31', false, false),
(23, '2023-12-24', '2023-12-31', true, true),
(24, '2023-12-24', '2023-12-31', false, true),
(25, '2023-12-24', '2023-12-31', true, false),
(26, '2023-12-24', '2023-12-31', false, false),
(27, '2023-12-31', '2024-01-03', true, true),
(28, '2023-12-31', '2024-01-05', false, true),
(29, '2023-12-31', '2024-01-07', true, false),
(30, '2024-01-02', '2024-01-06', false, false),
(31, '2024-01-03', '2024-01-07', true, true),
(32, '2024-01-04', '2024-01-08', false, true),
(33, '2024-01-05', '2024-01-10', true, false),
(34, '2024-01-06', '2024-01-10', false, true),
(35, '2024-01-07', '2024-01-11', true, true),
(36, '2024-01-08', '2024-01-12', false, false),
(37, '2024-01-09', '2024-01-13', true, true),
(38, '2024-01-10', '2024-01-14', false, true),
(39, '2024-01-11', '2024-01-15', true, false),
(40, '2024-01-12', '2024-01-16', false, true),
(41, '2024-01-13', '2024-01-17', true, false),
(42, '2024-01-14', '2024-01-18', false, false),
(43, '2024-01-15', '2024-01-19', true, true),
(44, '2024-01-16', '2024-01-20', false, true),
(45, '2024-01-17', '2024-01-21', true, false),
(46, '2024-01-18', '2024-01-22', false, false),
(47, '2024-01-19', '2024-01-23', true, true),
(48, '2024-01-20', '2024-01-24', false, true),
(49, '2024-01-21', '2024-01-25', true, false),
(50, '2024-12-21', '2024-12-25', false, true),
(51, '2023-12-21', '2023-12-25', false, true),
(52, '2023-12-22', '2023-12-26', true, false),
(53, '2023-12-22', '2024-01-02', false, true),
(54, '2023-12-22', '2024-01-02', true, true),
(55, '2023-12-26', '2023-12-30', false, true),
(56, '2023-12-26', '2023-12-30', true, false),
(57, '2023-12-12', '2023-12-19', false, false),
(58, '2023-12-28', '2023-12-19', true, true),
(59, '2023-12-30', '2024-01-06', false, true),
(60, '2023-12-30', '2024-01-06', true, false),
(61, '2024-01-01', '2024-01-06', false, true),
(62, '2024-01-01', '2024-01-06', true, true),
(63, '2024-01-03', '2024-01-07', false, false),
(64, '2024-01-03', '2024-01-07', true, false),
(65, '2024-01-05', '2024-01-09', false, true);

INSERT INTO booking_x_rooms (booking_id, room_id)
VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7),
(8, 8),
(9, 9),
(10, 10),
(11, 11),
(12, 12),
(13, 13),
(14, 14),
(15, 15),
(16, 16),
(17, 17),
(18, 18),
(19, 19),
(20, 20),
(21, 21),
(22, 22),
(23, 23),
(24, 24),
(25, 25),
(26, 26),
(27, 27),
(28, 28),
(29, 29),
(30, 30),
(31, 31),
(32, 32),
(33, 33),
(34, 34),
(35, 35),
(36, 36),
(37, 37),
(38, 38),
(39, 39),
(40, 40),
(41, 41),
(42, 42),
(43, 43),
(44, 44),
(45, 45),
(46, 46),
(47, 47),
(48, 48),
(49, 49),
(50, 50),
(51, 49),
(52, 50),
(53, 47),
(54, 48),
(55, 1),
(56, 2),
(57, 3),
(58, 4),
(59, 5),
(60, 6),
(61, 7),
(62, 8),
(63, 9),
(64, 10),
(65, 11);

