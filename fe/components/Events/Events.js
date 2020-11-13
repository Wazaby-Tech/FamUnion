import React from 'react';
import Event from './Event/Event';

const Events = (props) => {
    return props.events.map(function(e) {
        <Event key={e.id} event={e} />
    });
}

export default Events;