/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * @format
 * @flow strict-local
 */

import React, { useEffect, useState } from 'react';
import {
  SafeAreaView,
  StyleSheet,
  ScrollView,
  View,
  Text,
  StatusBar,
  ActivityIndicator
} from 'react-native';

import {
  Colors,
} from 'react-native/Libraries/NewAppScreen';

import Reunions from './components/Reunions/Reunions';


const App: () => React$Node = () => {
  const [isLoading, setLoading] = useState(true);
  const [reunions, setReunions] = useState([]);
  
  useEffect(() => {
    fetch("https://b321afb172cb.ngrok.io/api/reunions")
    .then((response) => { return response.json(); })
    .then((responseJson) => { setReunions(responseJson || []); })
    .catch((reason) => {
      console.log(`ERROR fetching reunions: ${reason}`);
    })
    .finally(() => { setLoading(false); });
  }, [])

  return (
    <>
      <StatusBar barStyle="dark-content" />
      <SafeAreaView>
        <ScrollView
          contentInsetAdjustmentBehavior="automatic"
          style={styles.scrollView}>
          <View style={styles.body}>
            <View style={styles.sectionContainer}>
              <Text style={styles.sectionTitle}>FamUnion</Text>
              <Text style={styles.sectionDescription}>
                This is the FamUnion mobile app.
              </Text>
            </View>
          </View>
          <View style={styles.body}>
            <View style={styles.sectionContainer}>
              <Text style={styles.sectionTitle}>Reunions</Text>
              <Text style={styles.sectionDescription}>
              {isLoading ? <ActivityIndicator/> : <Reunions reunions={reunions} />}
              </Text>
            </View>
          </View>
        </ScrollView>
      </SafeAreaView>
    </>
  );
};

const styles = StyleSheet.create({
  scrollView: {
    backgroundColor: Colors.lighter,
  },
  body: {
    backgroundColor: Colors.white,
    
  },
  sectionContainer: {
    marginTop: 32,
    paddingHorizontal: 24,
  },
  sectionTitle: {
    fontSize: 24,
    fontWeight: '600',
    color: Colors.black,
  },
  sectionDescription: {
    marginTop: 8,
    fontSize: 18,
    fontWeight: '400',
    color: Colors.dark,
  },
  highlight: {
    fontWeight: '700',
  },
  footer: {
    color: Colors.dark,
    fontSize: 12,
    fontWeight: '600',
    padding: 4,
    paddingRight: 12,
    textAlign: 'right',
  },
});

export default App;
