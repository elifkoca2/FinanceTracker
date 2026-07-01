import { useState, useEffect } from "react";
import {useAuth} from '../context/AuthContext';
import axiosInstance from '../api/axiosInstance';
import Navbar from '../components/Navbar';
import WatchlistTable from '../components/WatchlistTable';
import AddStockModal from '../components/AddStockModal';

export default function DashboardPage(){
    const {firstName} = useAuth();
    const [watchlist, setWatchlist] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showAddModal, setShowAddModal] = useState(false);

    //Hisse listesini çekme 
    const fectWacthlist = async () => {
        try {
            const response = await axiosInstance.get('/watchlist');
            setWatchlist(response.data);
        }
        catch(err){
            console.error('Liste çekilemedi: ', err);
        }
        finally{
            setLoading(false);
        }
    };

    useEffect(()=>{
        fectWacthlist();
    }, []);

    //Fiyat yenileme 
    const handleRefresh = async (id) =>{
        try{
            const response = await axiosInstance.put(`/watchlist/${id}/refresh`);
            setWatchlist(prev =>
                prev.map(item => item.id === id ? response.data : item)
                );
        }
        catch(err){
            console.error('Fiyat güncellenemedi: ', err);
        }
    };

    //Hisse silme 
    const handleDelete = async (id) => {
        if(!window.confirm('Bu hisseyi silmek istediğinize emin misiniz?')) return;
        try{
            await axiosInstance.delete(`/watchlist/${id}`);
            setWatchlist(prev => prev.filter(item=> item.id !== id));
        }
        catch(err){
            console.error('Hisse silinemedi: ',err);
        }
    };

    //Hisse eklendi - listeyi yenileme 
    const handleStockAdded = (newStock) => {
        setWatchlist(prev=> [...prev, newStock]);
        setShowAddModal(false);
    };

    return(
        <div className="min-h-screen bg-gray-100">
            <Navbar/>

            <div className="max-w-6xl mx-auto px-4 py-8">
              {/* Karşılama + Ekle butonu */}
            <div className="flex items-center justify-between mb-6">
                <div>
                <h2 className="text-2xl font-bold text-gray-800">
                Merhaba, {firstName}! 👋
                </h2>
                <p className="text-gray-500 text-sm mt-1">
                Takip listenizdeki hisseler aşağıda
                </p>
                </div>
                <button
                onClick={() => setShowAddModal(true)}
                className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition flex items-center gap-2"
            >
                <span className="text-xl">+</span> Hisse Ekle
                  </button>
            </div>

             {/* Loading durumu */}
            {loading? (
                 <div className="text-center py-20 text-gray-400">
                 <p className="text-lg">Yükleniyor...</p>
                </div>
            ) : watchlist.length === 0 ? (
                  // Boş liste durumu
                <div className="text-center py-20 bg-white rounded-xl shadow-sm">
                <p className="text-4xl mb-3">📭</p>
                <p className="text-gray-500 text-lg">Henüz hisse eklemediniz.</p>
                <p className="text-gray-400 text-sm mt-1">
                    "Hisse Ekle" butonuyla başlayın.
                </p>
            </div>
            ) : (  // Hisse tablosu
                <WatchlistTable
                watchlist={watchlist}
                onRefresh={handleRefresh}
                onDelete={handleDelete}
                setWatchlist={setWatchlist}
                />
          )}
            </div>
             {/* Hisse Ekle Modal */}
            {showAddModal && (
                <AddStockModal
                onClose={() => setShowAddModal(false)}
                onAdded={handleStockAdded}
                />
      )}
        </div>
    );
}

