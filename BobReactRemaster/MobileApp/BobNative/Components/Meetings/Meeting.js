/* eslint-disable react-native/no-inline-styles */
import React, { useState, useEffect } from 'react';
//import 'react-native-gesture-handler';
import { Pressable, ActivityIndicator, Button, FlatList, Text, View, SafeAreaView, TextInput, Image, TouchableOpacity, StyleSheet, Dimensions } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import { ScrollView } from 'react-native-gesture-handler';
import { useAnimatedStyle, useSharedValue } from 'react-native-reanimated';
import { FontAwesomeIcon } from '@fortawesome/react-native-fontawesome'
import { faCheck, faTimes, faQuestion } from '@fortawesome/free-solid-svg-icons'
import configData from "../../settings.json";
export function Meeting(props) {

    const [Data, setData] = useState(props.Data);

    let ScreenHeight = Dimensions.get("window").height;
    let ScreenWidth = Dimensions.get("window").width;
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
        },
        VoteButton:
        {
            width: ScreenWidth / 2 - 1,
            alignItems: 'center',
            paddingTop: 5,
            paddingBottom: 5,
        },
        MeetingVoteBody:
        {
            flex: 1,
            flexDirection: 'row',
            flexWrap: "wrap"
        }
    })
    if (Data == null) {
        //other view only while loading cause it would show loading when in fact nothing was loading
        return <View style={styles.View}>
            <ActivityIndicator size="large" />
        </View>
    }
    if (Data == false) {
        return null;
    }

    const handleStateChange = (e) => {
        /*
        let my = Participations.find( e => e.isMe);
        let comment = "";
        if(e === 3)
        {
            comment = prompt("Kommentar eingeben", "");
        }
        var data = {
            ParticipationID: parseInt(my.id),
            State: parseInt(e),
            Info: comment
        };
        
        fetch("/Meeting/UpdateParticipation",{
            method: "POST",
            headers:{
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + getCookie("Token"),
            },
            body: JSON.stringify(data)
        }).then(response => {
            if(response.ok)
            {
                let save = Participations.map(x => {
                    if(x.isMe)
                    {
                        x.state = e;   
                    }
                    return x;
                })
                setParticipations(save);
            }
        });
        */
    }
    //TODO loop meetings and render body below
    //Add Meeting Component and rename THIS to MeetingsList or something
    var pressed = false;
    return (
        <View>
            <View style={styles.MeetingHeader}>
                <Text style={styles.MeetingHeaderText}>{Data.meetingDate} {Data.meetingStart} - {Data.meetingEnd}</Text>
            </View>
            <View style={styles.MeetingVoteBody}>
                <Pressable style={(pressed) => [{ backgroundColor: pressed ? 'rgba(76,96,116,0.5)' : 'rgba(76,96,116,1)' }, styles.VoteButton]} onPress={e => { handleStateChange(0) }}>
                    <Text>-</Text>
                </Pressable>
                <Pressable style={({ pressed }) => [{ backgroundColor: pressed ? 'rgba(44,155,22,0.5)' : 'rgba(44,155,22,1)' }, styles.VoteButton]} onPress={e => { handleStateChange(1) }}>
                    <FontAwesomeIcon icon={faCheck} />
                </Pressable>
                <Pressable style={({ pressed }) => [{ backgroundColor: pressed ? 'rgba(255,52,52,0.5)' : 'rgba(255,52,52,1)' }, styles.VoteButton]} onPress={e => { handleStateChange(2) }}>
                    <FontAwesomeIcon icon={faTimes} />
                </Pressable>
                <Pressable style={({ pressed }) => [{ backgroundColor: pressed ? 'rgba(216,241,22,0.5)' : 'rgba(216,241,22,1)' }, styles.VoteButton]} onPress={e => { handleStateChange(3) }}>
                    <FontAwesomeIcon icon={faQuestion} />
                </Pressable>
            </View>
        </View>
    );
}
export default Meeting;

