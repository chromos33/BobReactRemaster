/* eslint-disable react-native/no-inline-styles */
import React, { useState, useEffect } from 'react';
//import 'react-native-gesture-handler';
import { KeyboardAvoidingView, Text, View, TextInput, Image, TouchableOpacity, StyleSheet, Dimensions } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
export function Login(props) {


  const [login, setLogin] = useState("");
  const [loginempty, setLoginEmpty] = useState(false);
  const [passwd, setPassword] = useState("");
  const [passwdempty, setPasswordEmpty] = useState(false);

  const loadUserData = async (e) => {
    try {
      const userdata = await EncryptedStorage.getItem("UserData");
      if (userdata != undefined) {
        let data = JSON.parse(userdata);
        
        setLogin(data.username);
        setPassword(data.passwd);
      }
    } catch (error) {
      console.log(error);
    }
  }
  loadUserData();
  const checkFakeForm = async (e) => {
    if (login === "") {
      setLoginEmpty(true);
      setTimeout(() => { setLoginEmpty(false) }, 200);
    }
    if (passwd === "") {
      setPasswordEmpty(true);
      setTimeout(() => { setPasswordEmpty(false) }, 200);
    }
    if (login !== "" && passwd !== "") {
      handleLogin();
    }
  }
  const handleLogin = async (evt) => {

    console.log("test");
    var loginResult = await fetch("http://192.168.50.243:5000/User/Login", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        username: login,
        password: passwd,
      }),
    }).then(response => {
      return response.json();
    }).then(json => {
      if (json.token !== undefined) {
        return json.token;
      }
      return false;
    }).catch((error) => {
      console.log(error);
    })
    console.log(loginResult);
    if (loginResult !== false) {
      try {
        await EncryptedStorage.setItem("UserData", JSON.stringify({
          username: login,
          passwd: passwd,
          token: loginResult
        }));
        props.navigation.navigate('main');
      } catch (error) {
        console.log(error);
        //TODO Error Message
      }

      //setCookie("Token", loginResult, 30);
      //history.push("/Subscriptions");
    }
    else {
      setLoginEmpty(true);
      setTimeout(() => { setLoginEmpty(false) }, 200);
      setPasswordEmpty(true);
      setTimeout(() => { setPasswordEmpty(false) }, 200);
    }
  }
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
    Logo: {
      marginBottom: 20
    },
    TextInput: {
      borderColor: "#486581",
      color: "#000",
      backgroundColor: "#9FB3C8",
      width: ScreenWidth * 0.8,
      marginBottom: 20,
      textAlign: 'center'
    },
    Button: {
      backgroundColor: "#334E68",
      width: ScreenWidth * 0.8,
      textAlign: 'center',
      paddingBottom: 10,
      paddingTop: 10
    },
    ButtonText: {
      color: '#9FB3C8',
      textAlign: 'center'
    }
  });
  return (
    <KeyboardAvoidingView behavior='padding' style={styles.MainView}>
      <View style={styles.CenteredView}>
        <Image style={styles.Logo} source={require("../Images/BobDeathmicLogo.png")} />
        <TextInput style={styles.TextInput} placeholderTextColor={"#5e6d7d"} placeholder="Login" onChangeText={e => { setLogin(e) }} />
        <TextInput secureTextEntry={true} style={styles.TextInput} placeholderTextColor={"#5e6d7d"} placeholder="Passwort" onChangeText={e => { setPassword(e) }} />
        <TouchableOpacity onPress={checkFakeForm} style={styles.Button}>
          <Text style={styles.ButtonText}>Login</Text>
        </TouchableOpacity>
      </View>
    </KeyboardAvoidingView>
  );
}
export default Login;