/* eslint-disable react-native/no-inline-styles */
import React, { useState, useEffect } from 'react';
//import 'react-native-gesture-handler';
import { KeyboardAvoidingView,ActivityIndicator,Button,FlatList, Text, View,SafeAreaView, TextInput, Image, TouchableOpacity, StyleSheet, Dimensions } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import { ScrollView } from 'react-native-gesture-handler';
import { useAnimatedStyle, useSharedValue } from 'react-native-reanimated';
import MeetingsList, {Meeting} from './MeetingsList';
import configData from "../../settings.json";
export function MeetingsView(props) {
    
    const [Meetings,setMeetings] = useState(null);
    const [Init,setInit] = useState(false);
    const [Fetching,setFetching] = useState(false);
    const onRefresh = () => 
    {
        setFetching(true);
        loadUserData();
    }
    const loadUserData = async (e) => {
        const userdata = await EncryptedStorage.getItem("UserData");
        if(userdata != false)
        {
            try
            {
                const token = JSON.parse(userdata).token;
                fetch(configData.SERVER_URL+"/Meeting/GetMeetingsTemplates",{
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
                    setMeetings(json);
                    setFetching(false);
                }).catch(error => {
                    setFetching(false);
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
    let ScreenHeight = Dimensions.get("window").height;
    let ScreenWidth = Dimensions.get("window").width;
    const styles = StyleSheet.create({
        MainView: {
            backgroundColor: "#243B53",
            height: ScreenHeight
        }
    });
    
    if(Meetings == null)
    {
        return <View style={styles.MainView}>
            <ActivityIndicator size="large" />
        </View>
    }
  return (
    <SafeAreaView style={styles.MainView}>
        <FlatList onRefresh={() => onRefresh()} refreshing={Fetching} data={Meetings.meetingTemplates} renderItem={({item}) => <MeetingsList data={item}/>} keyExtractor={item => item.id}/>
    </SafeAreaView>
  );
}
export default MeetingsView;

