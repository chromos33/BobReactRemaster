/* eslint-disable react-native/no-inline-styles */
import React, { useState, useEffect, useRef } from 'react';
//import 'react-native-gesture-handler';
import { KeyboardAvoidingView, Text, View, TextInput, Image, TouchableOpacity, StyleSheet, Dimensions, Animated } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import configData from "../settings.json";
export function Login(props) {
  
  useEffect(() => {
    Animated.sequence({

    })
  })
  const [user, setLogin] = useState("");
  const [loginempty, setLoginEmpty] = useState(false);
  const [passwd, setPassword] = useState("");
  const [passwdempty, setPasswordEmpty] = useState(false);
  const [init,setInit] = useState(false);
  const [position] = useState(new Animated.Value(0));

  const loadUserData = async (e) => {
    try {
      const userdata = await EncryptedStorage.getItem("UserData");
      if (userdata != undefined) {
        let data = JSON.parse(userdata);
        
        setLogin(data.username);
        setPassword(data.passwd);
      }
    } catch (error) {
    }
  }
  if(!init)
  {
    setInit(true);
    loadUserData();
  }
  
  const checkFakeForm = async (e) => {
    console.log("check");
    if (user === "") {
      setLoginEmpty(true);
      setTimeout(() => { setLoginEmpty(false) }, 200);
    }
    if (passwd === "") {
      setPasswordEmpty(true);
      setTimeout(() => { setPasswordEmpty(false) }, 200);
    }
    if (user !== "" && passwd !== "") {
      handleLogin();
    }
  }
  
  const translation = useRef(new Animated.Value(0)).current;
  const LoginError = () => {
    Animated.sequence([
      Animated.timing(translation,{
        toValue: 10,
        useNativeDriver: true,
        duration:20
      }),
      Animated.timing(translation,{
        toValue: 0,
        useNativeDriver: true,
        duration:10
      }),
      Animated.timing(translation,{
        toValue: -10,
        useNativeDriver: true,
        duration:10
      }),
      Animated.timing(translation,{
        toValue: 0,
        useNativeDriver: true,
        duration:10
      }),
    ]).start();
  }
  const handleLogin = async (e) => {
    fetch(configData.SERVER_URL+"/User/Login", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        username: user,
        password: passwd,
      }),
    }).then(response => {
        return response.json();
    }).then(json => {
      if (json.token !== undefined) {
        try {
          EncryptedStorage.setItem("UserData", JSON.stringify({
            username: user,
            passwd: passwd,
            token: json.token
          }));
          props.navigation.navigate('main');
        } catch (error) {
        }
        return true;
      }
      LoginError();
      return false;
    }).catch((error) => {
      LoginError();
    })
  }
  let ScreenHeight = Dimensions.get("window").height;
  let ScreenWidth = Dimensions.get("window").width;
  const styles = StyleSheet.create({
    MainView: {
      backgroundColor: configData.DARK_COLOR,
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
      borderColor: configData.LIGHTER_COLOR,
      color: configData.FONT_COLOR,
      backgroundColor: configData.LIGHT_COLOR,
      width: ScreenWidth * 0.8,
      marginBottom: 20,
      textAlign: 'center',
    },
    AnimatedView: {
      transform: [{translateX: translation}]
    },
    Button: {
      backgroundColor: configData.LIGHT_COLOR,
      width: ScreenWidth * 0.8,
      textAlign: 'center',
      paddingBottom: 10,
      paddingTop: 10
    },
    ButtonText: {
      color: configData.FONT_COLOR,
      textAlign: 'center'
    }
  });
  return (
    <KeyboardAvoidingView behavior='padding' style={styles.MainView}>
      <View style={styles.CenteredView}>
        <Image style={styles.Logo} source={require("../Images/BobDeathmicLogo.png")} />
        <Animated.View style={styles.AnimatedView}>
        <TextInput style={styles.TextInput} placeholderTextColor={"#5e6d7d"} placeholder="Login" value={user} onChangeText={e => { setLogin(e) }} />
        <TextInput secureTextEntry={true} style={styles.TextInput} placeholderTextColor={"#5e6d7d"} placeholder="Passwort" value={passwd} onChangeText={e => { setPassword(e) }} />
        </Animated.View>
        <TouchableOpacity onPress={e => {checkFakeForm();}} style={styles.Button}>
          <Text style={styles.ButtonText}>Login</Text>
        </TouchableOpacity>
      </View>
    </KeyboardAvoidingView>
  );
}
export default Login;