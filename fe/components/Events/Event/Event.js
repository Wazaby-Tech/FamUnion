import React from 'react';

const Event = (props) => {
    return (
        <div>
            <h1>{props.event.name}</h1>
            <p>{props.event.startDate}</p>
        </div>
    )
}

export default Event;