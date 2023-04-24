import React from 'react';
import PropTypes from 'prop-types';
import './LocationItemsCard.css';


const LocationItemsCard = ({locationItemsData: locationItemsData, selected, handleClick}) => {
    return (
        <div className="location-items-card" onClick={handleClick}>
            <div className="card-body">
                <div className="card-subtitle">{locationItemsData.identifier}</div>
            </div>
        </div>
    );
};

LocationItemsCard.propTypes = {
    locationItemsData: PropTypes.shape({
        identifier: PropTypes.string.isRequired,
    }).isRequired,
    selected: PropTypes.bool.isRequired,
};

export default LocationItemsCard;
