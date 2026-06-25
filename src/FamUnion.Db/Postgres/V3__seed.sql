INSERT INTO entity_type (entity_type_id, entity_name, is_active) VALUES
    (1, 'Reunion', TRUE),
    (2, 'Event',   TRUE),
    (3, 'Family',  FALSE),
    (4, 'Lodging', TRUE)
ON CONFLICT (entity_type_id) DO UPDATE
    SET is_active = EXCLUDED.is_active;
