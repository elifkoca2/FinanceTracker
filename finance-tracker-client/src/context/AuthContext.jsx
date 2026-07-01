import { createContext, useContext, useState } from "react";

const AuthContext= createContext(null);

//Kullanıcı giriş yönetimi 
export const AuthProvider= ({children}) => {
    const [token, setToken] = useState(localStorage.getItem('token'));
    const [firstName, setFirstName] = useState(localStorage.getItem('firstName'));

    const login =(token, firstName)=>{
        localStorage.setItem('token', token);
        localStorage.setItem('firstName', firstName);
        setToken(token);
        setFirstName(firstName);
    };

    const logout = () =>{
        localStorage.removeItem('token');
        localStorage.removeItem('firstName');
        setToken(null);
        setFirstName(null);
    };
    
    return(
        <AuthContext.Provider value={{token, firstName, login, logout}}>
            {children}
        </AuthContext.Provider>
    );
};

//Custom hook - her yerden kolay kullanmak için 
export const useAuth = ()=> useContext(AuthContext);