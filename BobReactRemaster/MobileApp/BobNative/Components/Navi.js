/* eslint-disable react-native/no-inline-styles */
import React, { useState, useEffect } from 'react';
//import 'react-native-gesture-handler';
import { KeyboardAvoidingView, Text, View, TextInput, Image, TouchableOpacity, StyleSheet, Dimensions } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import { useAnimatedStyle, useSharedValue } from 'react-native-reanimated';
import { NavigationContainer } from '@react-navigation/native';
import configData from "../settings.json";
import {MeetingsView} from './Meetings/View';
import {
  createDrawerNavigator,
  DrawerContentScrollView,
  DrawerItemList,
  DrawerItem,
} from '@react-navigation/drawer';
const Drawer = createDrawerNavigator();

const CustomDrawer = props => {
  return (<DrawerContentScrollView style={{
    backgroundColor:configData.DARK_COLOR,
    }} {...props}>
    <DrawerItemList {...props} />

  </DrawerContentScrollView>);
};

export function Navi(props) {
  return (
    <Drawer.Navigator screenOptions={{
       headerShown: true,
       headerTintColor: configData.FONT_COLOR,
       drawerActiveBackgroundColor: configData.LIGHTER_COLOR,
       drawerInactiveBackgroundColor: configData.DARK_COLOR,
       drawerLabelStyle: {color: configData.FONT_COLOR,lineHeight:14,fontSize:14,height:14},
       headerStyle: {
          backgroundColor: configData.LIGHT_COLOR,
          shadowOpacity: 0,
          elevation: 0,

         }
       }}  
       drawerContent={(props) => <CustomDrawer  {...props}/>}>
      <Drawer.Screen
        name="Meetings"
        component={MeetingsView}
        options={{ drawerLabel: 'Meetings 1' }}
      />
      <Drawer.Screen
        name="Meetings 2"
        component={MeetingsView}
        options={{ drawerLabel: 'Meetings 2' }}
      /> 
    </Drawer.Navigator>
  );
}

export default Navi;

