/* eslint-disable react-native/no-inline-styles */
import React, { useState, useEffect } from 'react';
//import 'react-native-gesture-handler';
import { KeyboardAvoidingView, Text, View, TextInput, Image, TouchableOpacity, StyleSheet, Dimensions } from 'react-native';
import EncryptedStorage from 'react-native-encrypted-storage';
import { useAnimatedStyle, useSharedValue } from 'react-native-reanimated';
import { NavigationContainer } from '@react-navigation/native';
import {MeetingsView} from './Meetings/View';
import {
  createDrawerNavigator,
  DrawerContentScrollView,
  DrawerItemList,
  DrawerItem,
} from '@react-navigation/drawer';
const Drawer = createDrawerNavigator();
export function Navi(props) {
  
  return (
    <Drawer.Navigator>
      <Drawer.Screen
        name="Meetings"
        component={MeetingsView}
        options={{ drawerLabel: 'Meetings' }}
      />
    </Drawer.Navigator>
  );
}
export default Navi;

