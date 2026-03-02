// This file serves as a central hub for re-exporting pre-typed Redux hooks.
import { useDispatch, useSelector, useStore } from "react-redux";


// Use throughout your app instead of plain `useDispatch` and `useSelector`
export const useAppDispatch = useDispatch;
export const useAppSelector = useSelector;
export const useAppStore = useStore;
