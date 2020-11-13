import React from 'react';
import {View, Text} from 'react-native';

const Reunion = (props) => {
    console.log(props.reunion);
    return (
        <View>
            <Text>{props.reunion.name}</Text>
            <Text>{props.reunion.description}</Text>
        </View>
    );
}

export default Reunion;