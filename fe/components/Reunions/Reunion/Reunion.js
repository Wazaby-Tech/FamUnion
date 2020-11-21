import React from 'react';
import {View, Text} from 'react-native';

const Reunion = (props) => {
    let events = props.reunion.events;
    let eventCount = (events || []).length;
    return (
        <View>
            <Text>{props.reunion.name} ({eventCount})</Text>
        </View>
    );
}

export default Reunion;