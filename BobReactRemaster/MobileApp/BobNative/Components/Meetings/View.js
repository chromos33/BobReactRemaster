/* eslint-disable react-native/no-inline-styles */
import React, { useState, useEffect } from 'react';
//import 'react-native-gesture-handler';
import { KeyboardAvoidingView, Text, View, TextInput, Image, TouchableOpacity, StyleSheet, Dimensions } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import { useAnimatedStyle, useSharedValue } from 'react-native-reanimated';
export function MeetingsView(props) {
    
    const loadUserData = async (e) => {
        const userdata = await EncryptedStorage.getItem("UserData");
        const token = JSON.parse(userdata).token;
        fetch("http://192.168.50.243:5000/Meeting/GetMeetingsTemplates",{
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token,
            }
        })
        .then(response => {
            return response.json();
        })
        .then(json => {
            console.log(json);
        });
        console.log("test");
    }
    loadUserData();
    
  return (
    <View>
        <Text>Meetings</Text>
    </View>
  );
}
export default MeetingsView;

