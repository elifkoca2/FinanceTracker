import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useState, useEffect } from 'react';
import axiosInstance from '../api/axiosInstance';
import NotificationPanel from './NotificationPanel';

export default function Navbar(){
    const {firstName, logout} = useAuth();
    const navigate = useNavigate();
    const [unreadCount, setUnreadCount] = useState(0);
    const [showNotifications, setShowNotifications] = useState(false);

    //Bildirim sayısını çekmek için 
    const fetchUnreadCount = async ()=> {
        try{
            const response = await axiosInstance.get('/alert/unread-count');
            setUnreadCount(response.data.unreadCount);
        }
        catch(err){
            console.error('Bildirim sayısı alınamadı: ', err);
        }
    };

    useEffect(()=>{
        fetchUnreadCount();
        // her 30 sn bir güncelle 
        const interval = setInterval(fetchUnreadCount, 30000);
        return ()=>clearInterval(interval);
    }, []);

    const handleLogout = ()=>{
        logout();
        navigate('/login');
    };

    const handleNotificationClick =() => {
        setShowNotifications(!showNotifications);
        if(!showNotifications){
            //Panel açılınca tüm bildirimler okundu olacak 
            axiosInstance.put('/alert/read-all').then(()=>{
                setUnreadCount(0);
            });
        }
    };

    return (
        <nav className="bg-white shadow-sm sticky top-0 z-10">
            <div className="max-w-6xl mx-auto px-4 py-3 flex items-center justify-between">
                 {/* Logo */}
                <h1 className="text-xl font-bold text-blue-600">
                Finance Tracker 📈
                </h1>

                  {/* Sağ taraf */}
                <div className="flex items-center gap-4">
                {/* Bildirim butonu */}
                <div className="relative">
                    <button
                    onClick={handleNotificationClick}
                    className="relative p-2 text-gray-600 hover:text-blue-600 transition"
                    >
                    <span className="text-2xl">🔔</span>
                    {unreadCount > 0 && (
                        <span className="absolute -top-1 -right-1 bg-red-500 text-white text-xs rounded-full w-5 h-5 flex items-center justify-center">
                        {unreadCount > 9 ? '9+' : unreadCount}
                        </span>
                    )}
                    </button>
                      {/* Bildirim paneli */}
                    {showNotifications && (
                    <NotificationPanel
                        onClose={() => setShowNotifications(false)}
                    />
                    )}
            </div>
                {/* Kullanıcı adı */}
            <span className="text-gray-600 text-sm hidden sm:block">
                {firstName}
            </span>

            {/* Çıkış butonu */}
            <button
                onClick={handleLogout}
                className="bg-gray-100 text-gray-600 px-3 py-1.5 rounded-lg hover:bg-gray-200 transition text-sm"
            >
            Çıkış
          </button>
          </div>
      </div>
        </nav>
    );
}