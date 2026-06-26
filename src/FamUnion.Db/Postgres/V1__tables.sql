-- entity_type: no dependencies
CREATE TABLE entity_type (
    entity_type_id INT          NOT NULL PRIMARY KEY,
    entity_name    VARCHAR(255) NOT NULL,
    is_active      BOOLEAN      NOT NULL DEFAULT TRUE
);

-- app_user: no dependencies
CREATE TABLE app_user (
    id           UUID         NOT NULL PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id      VARCHAR(100) NOT NULL UNIQUE,
    first_name   VARCHAR(100),
    last_name    VARCHAR(100),
    email        VARCHAR(255) NOT NULL,
    phone_number VARCHAR(15),
    auth_type    INT          NOT NULL,
    created_by   VARCHAR(100) NOT NULL,
    created_date TIMESTAMP    NOT NULL DEFAULT NOW(),
    modified_by  VARCHAR(100),
    modified_date TIMESTAMP
);

-- address: depends on entity_type
CREATE TABLE address (
    address_id    UUID         NOT NULL PRIMARY KEY DEFAULT gen_random_uuid(),
    entity_id     UUID         NOT NULL,
    entity_type   INT          NOT NULL REFERENCES entity_type(entity_type_id),
    description   VARCHAR(255) NOT NULL,
    line1         VARCHAR(100),
    line2         VARCHAR(100),
    city          VARCHAR(100) NOT NULL,
    state         VARCHAR(2)   NOT NULL,
    zip_code      VARCHAR(5),
    latitude      BIGINT,
    longitude     BIGINT,
    is_active     BOOLEAN      NOT NULL DEFAULT TRUE,
    created_by    VARCHAR(100) NOT NULL,
    created_date  TIMESTAMP    NOT NULL DEFAULT NOW(),
    modified_by   VARCHAR(100),
    modified_date TIMESTAMP
);

CREATE INDEX ix_address_entity ON address(entity_id, entity_type) WHERE is_active = TRUE;

-- reunion: depends on address
CREATE TABLE reunion (
    reunion_id    UUID          NOT NULL PRIMARY KEY DEFAULT gen_random_uuid(),
    name          VARCHAR(100)  NOT NULL,
    description   VARCHAR(4000),
    start_date    DATE,
    end_date      DATE,
    address_id    UUID          REFERENCES address(address_id),
    is_active     BOOLEAN       NOT NULL DEFAULT TRUE,
    created_by    VARCHAR(100)  NOT NULL DEFAULT CURRENT_USER,
    created_date  TIMESTAMP     NOT NULL DEFAULT NOW(),
    modified_by   VARCHAR(100),
    modified_date TIMESTAMP
);

-- events: depends on reunion and address
CREATE TABLE events (
    event_id      UUID          NOT NULL PRIMARY KEY DEFAULT gen_random_uuid(),
    reunion_id    UUID          NOT NULL REFERENCES reunion(reunion_id),
    name          VARCHAR(255)  NOT NULL,
    details       VARCHAR(2000),
    start_time    TIMESTAMP     NOT NULL,
    end_time      TIMESTAMP,
    attire_type   INT           NOT NULL DEFAULT 0,
    address_id    UUID          REFERENCES address(address_id),
    is_active     BOOLEAN       NOT NULL DEFAULT TRUE,
    created_by    VARCHAR(100)  NOT NULL,
    created_date  TIMESTAMP     NOT NULL DEFAULT NOW(),
    modified_by   VARCHAR(100),
    modified_date TIMESTAMP
);

-- reunion_invite: depends on reunion
CREATE TABLE reunion_invite (
    invite_id     UUID         NOT NULL PRIMARY KEY DEFAULT gen_random_uuid(),
    reunion_id    UUID         NOT NULL REFERENCES reunion(reunion_id),
    email         VARCHAR(255) NOT NULL,
    name          VARCHAR(255),
    rsvp_count    INT          NOT NULL DEFAULT 0,
    status        INT          NOT NULL DEFAULT 0,
    created_by    VARCHAR(100) NOT NULL DEFAULT CURRENT_USER,
    created_date  TIMESTAMP    NOT NULL DEFAULT NOW(),
    modified_by   VARCHAR(100),
    modified_date TIMESTAMP,
    UNIQUE (reunion_id, email)
);

-- reunion_organizer: depends on reunion and app_user
CREATE TABLE reunion_organizer (
    reunion_id UUID    NOT NULL REFERENCES reunion(reunion_id),
    user_id    UUID    NOT NULL REFERENCES app_user(id),
    is_active  BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY (reunion_id, user_id)
);
