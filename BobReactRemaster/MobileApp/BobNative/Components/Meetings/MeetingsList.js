/* eslint-disable react-native/no-inline-styles */
import React, { useState, useEffect } from 'react';
//import 'react-native-gesture-handler';
import { Pressable,ActivityIndicator, Text, View,SafeAreaView, StyleSheet, Dimensions } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import { FontAwesomeIcon } from '@fortawesome/react-native-fontawesome'
import { faCheck, faTimes,faQuestion } from '@fortawesome/free-solid-svg-icons'
import configData from "../../settings.json";
import Meeting from "./Meeting";
export function MeetingsList(props) {

    const [Data,setData] = useState(null);
    const [Init,setInit] = useState(false);
    const [token,setToken]  = useState(null);
    const loadUserData = async (e) => {
        const userdata = await EncryptedStorage.getItem("UserData");
        if(userdata != false)
        {
            try
            {
                const token = JSON.parse(userdata).token;
                setToken(token);
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
                    console.log("json");
                    console.log(json);
                    if(json[0] != undefined)
                    {
                        setData(json);
                    }
                    else
                    {
                        setData(false);
                    }
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
            paddingTop:5,
            paddingBottom:5,
        },
        MeetingVoteBody:
        {
            flex:1,
            flexDirection: 'row',
            flexWrap: "wrap"
        }
    })
    if(Data == null)
    {
        //other view only while loading cause it would show loading when in fact nothing was loading
        return <View style={styles.View}>
            <ActivityIndicator size="large" />
        </View>
    }
    if(Data == false)
    {
        return null;
    }
    console.log(Data);
    const handleStateChange = (state,meetingId,participationID,comment = "") => {
        var data = {
            ParticipationID: parseInt(participationID),
            State: parseInt(state),
            Info: comment
        };
        
        fetch(configData.SERVER_URL+"/Meeting/UpdateParticipation",{
            method: "POST",
            headers:{
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token,
            },
            body: JSON.stringify(data)
        }).then(response => {
            if(response.ok)
            {
                let save = Data.map(meeting => {
                    if(meeting.meetingID == meetingId)
                    {
                        meeting.meetingParticipations = meeting.meetingParticipations.map(part => {
                            if(part.isMe)
                            {
                                part.state = state;
                            }
                            return part;
                        });
                    }
                    return meeting;
                })
                setData(save);
                /*
                let save = Participations.map(x => {
                    if(x.isMe)
                    {
                        x.state = e;   
                    }
                    return x;
                })
                setParticipations(save);
                */
            }
        }).catch(e => {
            console.log(e);
        });
    }
    //TODO loop meetings and render body below
    //Add Meeting Component and rename THIS to MeetingsList or something
    const renderMeetings = () => {
        return Data.map((x,index) => {
            return <Meeting handleStateChange={handleStateChange} Data={x} key={index} />
            return (<View key={index}>
                <View style={styles.MeetingHeader}><Text style={styles.MeetingHeaderText}>{x.meetingDate} {x.meetingStart} - {x.meetingEnd}</Text></View>
                <View style={styles.MeetingVoteBody}>
                        <Pressable style={({pressed}) => [{
                            backgroundColor: pressed ? 'rgba(76,96,116,0.5)' : 'rgba(76,96,116,1)'
                        },
                        styles.VoteButton
                        ]} onPress={e => {handleStateChange(0)}}>
                            <Text>-</Text>
                        </Pressable>
                        <Pressable style={({pressed}) => [{
                            backgroundColor: pressed ? 'rgba(44,155,22,0.5)' : 'rgba(44,155,22,1)'
                        },
                        styles.VoteButton
                        ]} onPress={e => {handleStateChange(1)}}>
                            <FontAwesomeIcon icon={faCheck}/>
                        </Pressable>
                        <Pressable style={({pressed}) => [{
                            backgroundColor: pressed ? 'rgba(255,52,52,0.5)' : 'rgba(255,52,52,1)'
                        },
                        styles.VoteButton
                        ]} onPress={e => {handleStateChange(2)}}>
                            <FontAwesomeIcon icon={faTimes}/>
                        </Pressable>
                        <Pressable style={({pressed}) => [{
                            backgroundColor: pressed ? 'rgba(216,241,22,0.5)' : 'rgba(216,241,22,1)'
                        },
                        styles.VoteButton
                        ]} onPress={e => {handleStateChange(3)}}>
                            <FontAwesomeIcon icon={faQuestion}/>
                        </Pressable>
                </View>
            </View>);
        });
    }
  return (
    <SafeAreaView style={styles.View}>
       {renderMeetings()}
    </SafeAreaView>
  );
}
export default MeetingsList;

