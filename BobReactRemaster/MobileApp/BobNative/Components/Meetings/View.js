/* eslint-disable react-native/no-inline-styles */
import React, { useState, useEffect } from 'react';
//import 'react-native-gesture-handler';
import { KeyboardAvoidingView,Button,FlatList, Text, View,SafeAreaView, TextInput, Image, TouchableOpacity, StyleSheet, Dimensions } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import { ScrollView } from 'react-native-gesture-handler';
import { useAnimatedStyle, useSharedValue } from 'react-native-reanimated';
export function MeetingsView(props) {
    
    const [Meetings,setMeetings] = useState(null);
    const [Init,setInit] = useState(false);

    const loadUserData = async (e) => {
        const userdata = await EncryptedStorage.getItem("UserData");
        if(userdata != false)
        {
            try
            {
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
                    setMeetings(json)
                }).catch(error => {
                    
                });
            }catch(ex)
            {
                console.log(ex);
            }
            
           
        }
        
    }
    if(!Init)
    {
        setInit(true);
        loadUserData();
    }
    console.log(Meetings);
    const renderItem = ({item}) => (
        <Item item={item}/>
    );
  return (
    <SafeAreaView>
        <Button title="Test" onPress={loadUserData}></Button>
        <FlatList data={Meetings.meetingTemplates} renderItem={renderItem} keyExtractor={item => item.id}/>
    </SafeAreaView>
  );
}
const Item = ({item}) => (<View><Text>Test</Text></View>);
export default MeetingsView;

