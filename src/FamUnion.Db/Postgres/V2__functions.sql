-- ============================================================
-- Reunion functions
-- ============================================================

CREATE OR REPLACE FUNCTION sp_get_reunion_by_id(p_id UUID)
RETURNS TABLE (
    id            UUID,
    name          VARCHAR,
    description   VARCHAR,
    start_date    DATE,
    end_date      DATE,
    address_id    UUID,
    created_by    VARCHAR,
    created_date  TIMESTAMP,
    modified_by   VARCHAR,
    modified_date TIMESTAMP
)
LANGUAGE sql AS $$
    SELECT
        reunion_id,
        name,
        description,
        start_date,
        end_date,
        address_id,
        created_by,
        created_date,
        modified_by,
        modified_date
    FROM reunion
    WHERE reunion_id = p_id
      AND is_active = TRUE;
$$;

CREATE OR REPLACE FUNCTION sp_get_reunions()
RETURNS TABLE (
    id            UUID,
    name          VARCHAR,
    description   VARCHAR,
    start_date    DATE,
    end_date      DATE,
    address_id    UUID,
    created_by    VARCHAR,
    created_date  TIMESTAMP,
    modified_by   VARCHAR,
    modified_date TIMESTAMP
)
LANGUAGE sql AS $$
    SELECT
        reunion_id,
        name,
        description,
        start_date,
        end_date,
        address_id,
        created_by,
        created_date,
        modified_by,
        modified_date
    FROM reunion
    WHERE is_active = TRUE
    ORDER BY start_date, name;
$$;

CREATE OR REPLACE FUNCTION sp_get_manage_reunions(p_user_id VARCHAR)
RETURNS TABLE (
    id            UUID,
    name          VARCHAR,
    description   VARCHAR,
    start_date    DATE,
    end_date      DATE,
    address_id    UUID,
    created_by    VARCHAR,
    created_date  TIMESTAMP,
    modified_by   VARCHAR,
    modified_date TIMESTAMP
)
LANGUAGE sql AS $$
    SELECT
        r.reunion_id,
        r.name,
        r.description,
        r.start_date,
        r.end_date,
        r.address_id,
        r.created_by,
        r.created_date,
        r.modified_by,
        r.modified_date
    FROM reunion r
    JOIN reunion_organizer ro ON ro.reunion_id = r.reunion_id
    JOIN app_user u           ON ro.user_id = u.id
    WHERE r.is_active = TRUE
      AND u.user_id = p_user_id
    ORDER BY r.start_date, r.name;
$$;

CREATE OR REPLACE FUNCTION sp_save_reunion(
    p_id          UUID,
    p_user_id     VARCHAR,
    p_name        VARCHAR,
    p_description VARCHAR,
    p_start_date  DATE,
    p_end_date    DATE
)
RETURNS TABLE (
    id            UUID,
    name          VARCHAR,
    description   VARCHAR,
    start_date    DATE,
    end_date      DATE,
    address_id    UUID,
    created_by    VARCHAR,
    created_date  TIMESTAMP,
    modified_by   VARCHAR,
    modified_date TIMESTAMP
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO reunion (reunion_id, name, description, start_date, end_date, created_date, created_by)
    VALUES (p_id, p_name, p_description, p_start_date, p_end_date, NOW(), p_user_id)
    ON CONFLICT (reunion_id) DO UPDATE
        SET name          = EXCLUDED.name,
            description   = EXCLUDED.description,
            start_date    = EXCLUDED.start_date,
            end_date      = EXCLUDED.end_date,
            modified_by   = p_user_id,
            modified_date = NOW()
        WHERE reunion.is_active = TRUE;

    RETURN QUERY SELECT * FROM sp_get_reunion_by_id(p_id);
END;
$$;

CREATE OR REPLACE FUNCTION sp_delete_reunion_by_id(p_reunion_id UUID)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE address
    SET is_active     = FALSE,
        modified_by   = CURRENT_USER,
        modified_date = NOW()
    WHERE entity_id   = p_reunion_id
      AND entity_type = (SELECT entity_type_id FROM entity_type WHERE entity_name = 'Reunion');

    UPDATE reunion
    SET is_active     = FALSE,
        modified_by   = CURRENT_USER,
        modified_date = NOW()
    WHERE reunion_id  = p_reunion_id;
END;
$$;

CREATE OR REPLACE FUNCTION sp_add_reunion_organizer(p_reunion_id UUID, p_email VARCHAR)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO reunion_organizer (reunion_id, user_id)
    SELECT p_reunion_id, id
    FROM app_user
    WHERE email = p_email
    ON CONFLICT (reunion_id, user_id) DO UPDATE
        SET is_active = TRUE;
END;
$$;

CREATE OR REPLACE FUNCTION sp_remove_reunion_organizer(p_reunion_id UUID, p_email VARCHAR)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE reunion_organizer ro
    SET is_active = FALSE
    FROM app_user u
    WHERE ro.reunion_id = p_reunion_id
      AND ro.user_id    = u.id
      AND u.email       = p_email;
END;
$$;

-- ============================================================
-- Event functions
-- ============================================================

CREATE OR REPLACE FUNCTION sp_get_event_by_id(p_id UUID)
RETURNS TABLE (
    id            UUID,
    reunion_id    UUID,
    name          VARCHAR,
    details       VARCHAR,
    start_time    TIMESTAMP,
    end_time      TIMESTAMP,
    attire_type   INT,
    address_id    UUID,
    created_by    VARCHAR,
    created_date  TIMESTAMP,
    modified_by   VARCHAR,
    modified_date TIMESTAMP
)
LANGUAGE sql AS $$
    SELECT
        event_id,
        reunion_id,
        name,
        details,
        start_time,
        end_time,
        attire_type,
        address_id,
        created_by,
        created_date,
        modified_by,
        modified_date
    FROM events
    WHERE event_id = p_id;
$$;

CREATE OR REPLACE FUNCTION sp_get_events_by_reunion_id(p_reunion_id UUID)
RETURNS TABLE (
    id            UUID,
    reunion_id    UUID,
    name          VARCHAR,
    details       VARCHAR,
    start_time    TIMESTAMP,
    end_time      TIMESTAMP,
    attire_type   INT,
    address_id    UUID,
    created_by    VARCHAR,
    created_date  TIMESTAMP,
    modified_by   VARCHAR,
    modified_date TIMESTAMP
)
LANGUAGE sql AS $$
    SELECT
        event_id,
        reunion_id,
        name,
        details,
        start_time,
        end_time,
        attire_type,
        address_id,
        created_by,
        created_date,
        modified_by,
        modified_date
    FROM events
    WHERE reunion_id = p_reunion_id
    ORDER BY start_time, name;
$$;

CREATE OR REPLACE FUNCTION sp_save_event(
    p_user_id    VARCHAR,
    p_id         UUID,
    p_reunion_id UUID,
    p_name       VARCHAR,
    p_details    VARCHAR,
    p_start_time TIMESTAMP,
    p_end_time   TIMESTAMP,
    p_attire_type INT
)
RETURNS TABLE (
    id            UUID,
    reunion_id    UUID,
    name          VARCHAR,
    details       VARCHAR,
    start_time    TIMESTAMP,
    end_time      TIMESTAMP,
    attire_type   INT,
    address_id    UUID,
    created_by    VARCHAR,
    created_date  TIMESTAMP,
    modified_by   VARCHAR,
    modified_date TIMESTAMP
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO events (event_id, reunion_id, name, details, start_time, end_time, attire_type, created_date, created_by)
    VALUES (p_id, p_reunion_id, p_name, p_details, p_start_time, p_end_time, p_attire_type, NOW(), p_user_id)
    ON CONFLICT (event_id) DO UPDATE
        SET name          = EXCLUDED.name,
            reunion_id    = EXCLUDED.reunion_id,
            details       = EXCLUDED.details,
            start_time    = EXCLUDED.start_time,
            end_time      = EXCLUDED.end_time,
            attire_type   = EXCLUDED.attire_type,
            modified_by   = p_user_id,
            modified_date = NOW()
        WHERE events.is_active = TRUE;

    RETURN QUERY SELECT * FROM sp_get_event_by_id(p_id);
END;
$$;

CREATE OR REPLACE FUNCTION sp_cancel_event_by_id(p_user_id VARCHAR, p_event_id UUID)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE address
    SET is_active     = FALSE,
        modified_by   = p_user_id,
        modified_date = NOW()
    WHERE entity_id   = p_event_id
      AND entity_type = (SELECT entity_type_id FROM entity_type WHERE entity_name = 'Event');

    UPDATE events
    SET is_active     = FALSE,
        modified_by   = p_user_id,
        modified_date = NOW()
    WHERE event_id    = p_event_id;
END;
$$;

-- ============================================================
-- Address functions
-- ============================================================

CREATE OR REPLACE FUNCTION sp_get_address_by_entity_type_and_id(p_entity_type_id INT, p_entity_id UUID)
RETURNS TABLE (
    id            UUID,
    address_type  INT,
    description   VARCHAR,
    entity_type   INT,
    line1         VARCHAR,
    line2         VARCHAR,
    city          VARCHAR,
    state         VARCHAR,
    zip_code      VARCHAR,
    latitude      BIGINT,
    longitude     BIGINT,
    created_by    VARCHAR,
    created_date  TIMESTAMP,
    modified_by   VARCHAR,
    modified_date TIMESTAMP
)
LANGUAGE sql AS $$
    SELECT
        a.address_id,
        p_entity_type_id,
        a.description,
        a.entity_type,
        a.line1,
        a.line2,
        a.city,
        a.state,
        a.zip_code,
        a.latitude,
        a.longitude,
        a.created_by,
        a.created_date,
        a.modified_by,
        a.modified_date
    FROM address a
    WHERE a.entity_type = p_entity_type_id
      AND a.entity_id   = p_entity_id
      AND a.is_active   = TRUE;
$$;

CREATE OR REPLACE FUNCTION sp_get_address_by_event_id(p_event_id UUID)
RETURNS TABLE (
    id            UUID,
    address_type  INT,
    description   VARCHAR,
    entity_type   INT,
    line1         VARCHAR,
    line2         VARCHAR,
    city          VARCHAR,
    state         VARCHAR,
    zip_code      VARCHAR,
    latitude      BIGINT,
    longitude     BIGINT,
    created_by    VARCHAR,
    created_date  TIMESTAMP,
    modified_by   VARCHAR,
    modified_date TIMESTAMP
)
LANGUAGE sql AS $$
    SELECT * FROM sp_get_address_by_entity_type_and_id(
        (SELECT entity_type_id FROM entity_type WHERE entity_name = 'Event'),
        p_event_id
    );
$$;

CREATE OR REPLACE FUNCTION sp_get_address_by_reunion_id(p_reunion_id UUID)
RETURNS TABLE (
    id            UUID,
    address_type  INT,
    description   VARCHAR,
    entity_type   INT,
    line1         VARCHAR,
    line2         VARCHAR,
    city          VARCHAR,
    state         VARCHAR,
    zip_code      VARCHAR,
    latitude      BIGINT,
    longitude     BIGINT,
    created_by    VARCHAR,
    created_date  TIMESTAMP,
    modified_by   VARCHAR,
    modified_date TIMESTAMP
)
LANGUAGE sql AS $$
    SELECT * FROM sp_get_address_by_entity_type_and_id(
        (SELECT entity_type_id FROM entity_type WHERE entity_name = 'Reunion'),
        p_reunion_id
    );
$$;

CREATE OR REPLACE FUNCTION sp_save_address_by_entity_type_and_id(
    p_user_id     VARCHAR,
    p_entity_type VARCHAR,
    p_entity_id   UUID,
    p_description VARCHAR,
    p_line1       VARCHAR,
    p_line2       VARCHAR,
    p_city        VARCHAR,
    p_state       VARCHAR,
    p_zip_code    VARCHAR
)
RETURNS VOID
LANGUAGE plpgsql AS $$
DECLARE
    v_entity_type_id INT;
    v_address_id     UUID;
BEGIN
    SELECT entity_type_id INTO v_entity_type_id
    FROM entity_type
    WHERE entity_name = p_entity_type;

    SELECT address_id INTO v_address_id
    FROM address
    WHERE entity_id   = p_entity_id
      AND entity_type = v_entity_type_id
      AND is_active   = TRUE
    LIMIT 1;

    IF v_address_id IS NOT NULL THEN
        UPDATE address
        SET description   = p_description,
            line1         = p_line1,
            line2         = p_line2,
            city          = p_city,
            state         = p_state,
            zip_code      = p_zip_code,
            modified_by   = p_user_id,
            modified_date = NOW()
        WHERE address_id  = v_address_id;
    ELSE
        INSERT INTO address (
            address_id, entity_id, entity_type, description,
            line1, line2, city, state, zip_code,
            created_date, created_by
        )
        VALUES (
            gen_random_uuid(), p_entity_id, v_entity_type_id, p_description,
            p_line1, p_line2, p_city, p_state, p_zip_code,
            NOW(), p_user_id
        );
    END IF;
END;
$$;

CREATE OR REPLACE FUNCTION sp_save_reunion_address(
    p_user_id     VARCHAR,
    p_reunion_id  UUID,
    p_description VARCHAR,
    p_line1       VARCHAR,
    p_line2       VARCHAR,
    p_city        VARCHAR,
    p_state       VARCHAR,
    p_zip_code    VARCHAR
)
RETURNS TABLE (
    id            UUID,
    address_type  INT,
    description   VARCHAR,
    entity_type   INT,
    line1         VARCHAR,
    line2         VARCHAR,
    city          VARCHAR,
    state         VARCHAR,
    zip_code      VARCHAR,
    latitude      BIGINT,
    longitude     BIGINT,
    created_by    VARCHAR,
    created_date  TIMESTAMP,
    modified_by   VARCHAR,
    modified_date TIMESTAMP
)
LANGUAGE plpgsql AS $$
BEGIN
    PERFORM sp_save_address_by_entity_type_and_id(
        p_user_id, 'Reunion', p_reunion_id,
        p_description, p_line1, p_line2, p_city, p_state, p_zip_code
    );
    RETURN QUERY SELECT * FROM sp_get_address_by_reunion_id(p_reunion_id);
END;
$$;

CREATE OR REPLACE FUNCTION sp_save_event_address(
    p_user_id     VARCHAR,
    p_event_id    UUID,
    p_description VARCHAR,
    p_line1       VARCHAR,
    p_line2       VARCHAR,
    p_city        VARCHAR,
    p_state       VARCHAR,
    p_zip_code    VARCHAR
)
RETURNS TABLE (
    id            UUID,
    address_type  INT,
    description   VARCHAR,
    entity_type   INT,
    line1         VARCHAR,
    line2         VARCHAR,
    city          VARCHAR,
    state         VARCHAR,
    zip_code      VARCHAR,
    latitude      BIGINT,
    longitude     BIGINT,
    created_by    VARCHAR,
    created_date  TIMESTAMP,
    modified_by   VARCHAR,
    modified_date TIMESTAMP
)
LANGUAGE plpgsql AS $$
BEGIN
    PERFORM sp_save_address_by_entity_type_and_id(
        p_user_id, 'Event', p_event_id,
        p_description, p_line1, p_line2, p_city, p_state, p_zip_code
    );
    RETURN QUERY SELECT * FROM sp_get_address_by_event_id(p_event_id);
END;
$$;

CREATE OR REPLACE FUNCTION sp_delete_address_by_id(p_address_id UUID)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE address
    SET is_active     = FALSE,
        modified_by   = CURRENT_USER,
        modified_date = NOW()
    WHERE address_id  = p_address_id;
END;
$$;

-- ============================================================
-- User functions
-- ============================================================

CREATE OR REPLACE FUNCTION sp_get_user_by_id(p_user_id VARCHAR)
RETURNS TABLE (
    id           UUID,
    user_id      VARCHAR,
    first_name   VARCHAR,
    last_name    VARCHAR,
    email        VARCHAR,
    phone_number VARCHAR,
    auth_type    INT
)
LANGUAGE sql AS $$
    SELECT id, user_id, first_name, last_name, email, phone_number, auth_type
    FROM app_user
    WHERE user_id = p_user_id;
$$;

CREATE OR REPLACE FUNCTION sp_get_user_by_email(p_email VARCHAR)
RETURNS TABLE (
    id           UUID,
    user_id      VARCHAR,
    first_name   VARCHAR,
    last_name    VARCHAR,
    email        VARCHAR,
    phone_number VARCHAR,
    auth_type    INT
)
LANGUAGE sql AS $$
    SELECT id, user_id, first_name, last_name, email, phone_number, auth_type
    FROM app_user
    WHERE email = p_email;
$$;

CREATE OR REPLACE FUNCTION sp_save_user(
    p_id        UUID,
    p_user_id   VARCHAR,
    p_email     VARCHAR,
    p_first_name VARCHAR,
    p_last_name  VARCHAR,
    p_auth_type  INT
)
RETURNS TABLE (
    id           UUID,
    user_id      VARCHAR,
    first_name   VARCHAR,
    last_name    VARCHAR,
    email        VARCHAR,
    phone_number VARCHAR,
    auth_type    INT
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO app_user (id, user_id, email, first_name, last_name, auth_type, created_date, created_by)
    VALUES (p_id, p_user_id, p_email, p_first_name, p_last_name, p_auth_type, NOW(), CURRENT_USER)
    ON CONFLICT (user_id) DO UPDATE
        SET email         = EXCLUDED.email,
            first_name    = EXCLUDED.first_name,
            last_name     = EXCLUDED.last_name,
            modified_by   = CURRENT_USER,
            modified_date = NOW();

    RETURN QUERY SELECT * FROM sp_get_user_by_id(p_user_id);
END;
$$;

CREATE OR REPLACE FUNCTION sp_get_organizers_by_reunion_id(p_reunion_id UUID)
RETURNS TABLE (
    user_id      VARCHAR,
    first_name   VARCHAR,
    last_name    VARCHAR,
    email        VARCHAR,
    phone_number VARCHAR,
    auth_type    INT
)
LANGUAGE sql AS $$
    SELECT u.user_id, u.first_name, u.last_name, u.email, u.phone_number, u.auth_type
    FROM reunion_organizer ro
    JOIN app_user u ON ro.user_id = u.id AND ro.is_active = TRUE
    WHERE ro.reunion_id = p_reunion_id;
$$;

-- ============================================================
-- Attendee / invite functions
-- ============================================================

CREATE OR REPLACE FUNCTION sp_get_invites_by_reunion_id(p_reunion_id UUID)
RETURNS TABLE (
    id         UUID,
    reunion_id UUID,
    email      VARCHAR,
    name       VARCHAR,
    rsvp_count INT,
    status     INT
)
LANGUAGE sql AS $$
    SELECT invite_id, reunion_id, email, name, rsvp_count, status
    FROM reunion_invite
    WHERE reunion_id = p_reunion_id
    ORDER BY email;
$$;

CREATE OR REPLACE FUNCTION sp_get_invite_by_id(p_invite_id UUID)
RETURNS TABLE (
    id         UUID,
    reunion_id UUID,
    email      VARCHAR,
    name       VARCHAR,
    rsvp_count INT,
    status     INT
)
LANGUAGE sql AS $$
    SELECT invite_id, reunion_id, email, name, rsvp_count, status
    FROM reunion_invite
    WHERE invite_id = p_invite_id;
$$;

CREATE OR REPLACE FUNCTION sp_create_invites(p_invites TEXT, p_user_id VARCHAR)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO reunion_invite (invite_id, reunion_id, email, name, created_by, created_date)
    SELECT
        gen_random_uuid(),
        (item->>'reunion_id')::UUID,
        item->>'email',
        item->>'name',
        p_user_id,
        NOW()
    FROM jsonb_array_elements(p_invites::jsonb) AS item
    ON CONFLICT (reunion_id, email) DO NOTHING;
END;
$$;

CREATE OR REPLACE FUNCTION sp_update_invite_status(p_invite_id UUID, p_status INT)
RETURNS VOID
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE reunion_invite
    SET status        = p_status,
        modified_date = NOW(),
        modified_by   = CURRENT_USER
    WHERE invite_id   = p_invite_id;
END;
$$;

-- ============================================================
-- User access functions
-- ============================================================

CREATE OR REPLACE FUNCTION sp_user_has_read_access_to_entity(
    p_user_id    VARCHAR,
    p_entity_type INT,
    p_entity_id  UUID
)
RETURNS TABLE (result BOOLEAN)
LANGUAGE plpgsql AS $$
BEGIN
    -- Reunion entity
    IF p_entity_type = 1 THEN
        RETURN QUERY
        SELECT COUNT(1) > 0
        FROM reunion r
        JOIN reunion_organizer ro ON r.reunion_id = ro.reunion_id
            AND r.reunion_id = p_entity_id
            AND r.is_active  = TRUE
            AND ro.is_active = TRUE
        JOIN app_user u ON ro.user_id = u.id AND u.user_id = p_user_id;
    END IF;

    -- Event entity
    IF p_entity_type = 2 THEN
        RETURN QUERY
        SELECT COUNT(1) > 0
        FROM events e
        JOIN reunion r          ON e.reunion_id = r.reunion_id AND e.event_id = p_entity_id AND e.is_active = TRUE
        JOIN reunion_organizer ro ON r.reunion_id = ro.reunion_id AND ro.is_active = TRUE
        JOIN app_user u         ON ro.user_id = u.id AND u.user_id = p_user_id;
    END IF;
END;
$$;

CREATE OR REPLACE FUNCTION sp_user_has_write_access_to_entity(
    p_user_id     VARCHAR,
    p_entity_type INT,
    p_entity_id   UUID
)
RETURNS TABLE (result BOOLEAN)
LANGUAGE plpgsql AS $$
BEGIN
    -- Reunion entity
    IF p_entity_type = 1 THEN
        RETURN QUERY
        SELECT COUNT(1) > 0
        FROM reunion r
        JOIN reunion_organizer ro ON r.reunion_id = ro.reunion_id
            AND r.reunion_id = p_entity_id
            AND r.is_active  = TRUE
            AND ro.is_active = TRUE
        JOIN app_user u ON ro.user_id = u.id AND u.user_id = p_user_id;
    END IF;

    -- Event entity
    IF p_entity_type = 2 THEN
        RETURN QUERY
        SELECT COUNT(1) > 0
        FROM events e
        JOIN reunion r          ON e.reunion_id = r.reunion_id AND e.event_id = p_entity_id AND e.is_active = TRUE
        JOIN reunion_organizer ro ON r.reunion_id = ro.reunion_id AND ro.is_active = TRUE
        JOIN app_user u         ON ro.user_id = u.id AND u.user_id = p_user_id;
    END IF;
END;
$$;
