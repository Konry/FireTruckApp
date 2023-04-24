import React from 'react';
import PropTypes from 'prop-types';
import './FireTruckCard.css';

const FireTruckCard = ({fireTruckData, selected, handleClickTruck}) => {
    return (
        <div className="location-card" onClick={handleClickTruck}>
            <div className="card-body">
                <div className="card-title">{fireTruckData.truckTypeShort}</div>
                <div className="card-subtitle">{fireTruckData.identifier}</div>
            </div>
            <div className="image"
                 style={{backgroundImage: `url("${process.env.REACT_APP_API_HOST}Image?firetruck=${fireTruckData.identifier}")`}}/>
        </div>
    );
};

FireTruckCard.propTypes = {
    fireTruckData: PropTypes.shape({
        identifier: PropTypes.string.isRequired,
        truckType: PropTypes.string.isRequired,
        truckTypeShort: PropTypes.string.isRequired,
    }).isRequired,
    selected: PropTypes.bool.isRequired,
};

export default FireTruckCard;
