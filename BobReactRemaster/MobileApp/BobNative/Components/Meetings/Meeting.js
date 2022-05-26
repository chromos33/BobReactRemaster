/* eslint-disable react-native/no-inline-styles */
import React, { useState, useEffect } from 'react';
//import 'react-native-gesture-handler';
import { KeyboardAvoidingView,ActivityIndicator,Button,FlatList, Text, View,SafeAreaView, TextInput, Image, TouchableOpacity, StyleSheet, Dimensions } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import { ScrollView } from 'react-native-gesture-handler';
import { useAnimatedStyle, useSharedValue } from 'react-native-reanimated';
import configData from "../../settings.json";
export function Meeting(props) {

    const [Data,setData] = useState(null);
    const [Init,setInit] = useState(false);
    const loadUserData = async (e) => {
        const userdata = await EncryptedStorage.getItem("UserData");
        if(userdata != false)
        {
            try
            {
                console.log(props.data.id);
                const token = JSON.parse(userdata).token;
                fetch(configData.SERVER_URL+"/Meeting/GetMeetings?MeetingID="+ props.data.id,{
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
                    setData(json[0])
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
    const styles = StyleSheet.create({
        MeetingHeader: {
            backgroundColor: configData.DARK_COLOR,
            textAlign: 'center',
            paddingTop: 5,
            paddingBottom: 5,
            color: configData.FONT_COLOR
        },
        MeetingHeaderText:
        {
            textAlign: 'center',
            color: configData.FONT_COLOR
        },
        View:
        {
            backgroundColor: '#243B53'
        }
    })
    if(Data == null)
    {
        //other view only while loading cause it would show loading when in fact nothing was loading
        return <View style={styles.View}>
            <ActivityIndicator size="large" />
        </View>
    }
  return (
    <SafeAreaView style={styles.View}>
       <View style={styles.MeetingHeader}><Text style={styles.MeetingHeaderText}>{Data.meetingDate} {Data.meetingStart} - {Data.meetingEnd}</Text></View>
    </SafeAreaView>
  );
}
export default Meeting;

