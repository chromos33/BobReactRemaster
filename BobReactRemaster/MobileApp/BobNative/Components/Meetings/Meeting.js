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
import DialogInput from 'react-native-dialog-input';
export function Meeting(props) {

    const [Data, setData] = useState(props.Data);
    const [isDialogVisible, setisDialogVisible] = useState(false);
    const [tmpState,settmpState] = useState(0);

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
    if (props.Data == null) {
        //other view only while loading cause it would show loading when in fact nothing was loading
        return <View style={styles.View}>
            <ActivityIndicator size="large" />
        </View>
    }
    if (Data == false) {
        return null;
    }
    var pressed = false;
    const getMyParticipation = () => {
        return props.Data.meetingParticipations.filter(x => x.isMe)[0];
    }
    const isCurrentState = (e) => {
        let myparticipation = getMyParticipation();
        return e == myparticipation.state;

    };
    const handleCommentState = (state) => {
        settmpState(state);
        setisDialogVisible(true);
    }
    let state0 = isCurrentState(0);
    let state1 = isCurrentState(1);
    let state2 = isCurrentState(2);
    let state3 = isCurrentState(3);
    return (
        <View>
            <DialogInput
                isDialogVisible={isDialogVisible}
                title={'Hinweis'}
                message={'Hinweis eingeben'}
                hintInput={'Hinweis'}
                submitInput={inputText => {
                    props.handleStateChange(tmpState,props.Data.meetingID,getMyParticipation().id,inputText);
                    setisDialogVisible(false);
                }}
                closeDialog={() => {
                    setisDialogVisible(false);
                }}
            />
            <View style={styles.MeetingHeader}>
                <Text style={styles.MeetingHeaderText}>{props.Data.meetingDate} {props.Data.meetingStart} - {props.Data.meetingEnd}</Text>
            </View>
            <View style={styles.MeetingVoteBody}>
                <Pressable style={[{ backgroundColor: state0 ? 'rgba(76,96,116,1)' : 'rgba(76,96,116,0.1)' }, styles.VoteButton]} onPress={e => { props.handleStateChange(0,props.Data.meetingID,getMyParticipation().id); }}>
                    <Text>-</Text>
                </Pressable>
                <Pressable style={[{ backgroundColor: state1 ? 'rgba(44,155,22,1)' : 'rgba(44,155,22,0.1)' }, styles.VoteButton]} onPress={e => { props.handleStateChange(1,props.Data.meetingID,getMyParticipation().id) }}>
                    <FontAwesomeIcon icon={faCheck} />
                </Pressable>
                <Pressable style={[{ backgroundColor: state2 ? 'rgba(255,52,52,1)' : 'rgba(255,52,52,0.1)' }, styles.VoteButton]} onPress={e => { props.handleStateChange(2,props.Data.meetingID,getMyParticipation().id) }}>
                    <FontAwesomeIcon icon={faTimes} />
                </Pressable>
                <Pressable style={[{ backgroundColor: state3 ? 'rgba(216,241,22,1)' : 'rgba(216,241,22,0.1)' }, styles.VoteButton]} onPress={e => { handleCommentState(3) }}>
                    <FontAwesomeIcon icon={faQuestion} />
                </Pressable>
            </View>
        </View>
    );
}
export default Meeting;

