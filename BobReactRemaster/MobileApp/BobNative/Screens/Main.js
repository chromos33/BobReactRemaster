/* eslint-disable react-native/no-inline-styles */
import React, { useState, useEffect } from 'react';
//import 'react-native-gesture-handler';
import { KeyboardAvoidingView, Text, View, TextInput, Image, TouchableOpacity, StyleSheet, Dimensions } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
export function Main(props) {
  let ScreenHeight = Dimensions.get("window").height;
  let ScreenWidth = Dimensions.get("window").width;
  const styles = StyleSheet.create({
    MainView: {
      backgroundColor: "#243B53",
      height: ScreenHeight
    },
    CenteredView: {
      justifyContent: 'center',
      flex: 1,
      alignItems: 'center'
    },
    ButtonText: {
      color: '#9FB3C8',
      textAlign: 'center'
    }
  });
  return (
      <View style={styles.MainView}>
        <View style={styles.CenteredView}>
            <Text style={styles.ButtonText}>Test</Text>
        </View>
      </View>
  );
}
export default Main;