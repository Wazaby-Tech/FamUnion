import React from 'react';
import Reunion from './Reunion/Reunion';
import {FlatList, Text} from 'react-native';

const Reunions = (props) => {
    return props.reunions.length == 0 ? <Text>No reunions found</Text> : 
    <FlatList
        data={props.reunions}
        keyExtractor={({ id }, index) => id}
        renderItem={({ item }) => (
            <Reunion reunion={item} />
        )}
    />;
}

export default Reunions;