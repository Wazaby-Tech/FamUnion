import React from 'react';
import Reunion from './Reunion/Reunion';
import {Text} from 'react-native';

const Reunions = (props) => {

    return props.reunions.length == 0 ? <Text>No reunions found</Text> : 
        props.reunions.map(function(e){
        <Reunion key={e.id} reunion={e} />
    });
}

export default Reunions;